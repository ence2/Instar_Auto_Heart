using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Diagnostics;

namespace InstarAutoHeart
{
    public class Manager : Singleton<Manager>
    {
        private InstarAutoHeart _instartAutoHeart;

        private ChromeDriverService _driverService = null;
        private ChromeOptions _options = null;
        private ChromeDriver _driver = null;

        private Thread _worker = null;
        private WebDriverWait _wait = null;

        private TargetData _currentTarget = null;
        private Queue<string> _searchWords = new Queue<string>();

        private WorkerState worketState;
        private bool isIntercept = false;

        public void Init(ref InstarAutoHeart iah)
        {
            try
            {
                _instartAutoHeart = iah;

                // Reload data
                foreach (var item in Config.Instance.Data.priorityTags)
                {
                    _instartAutoHeart.tbTags.AppendText(item);
                    _instartAutoHeart.tbTags.AppendText(System.Environment.NewLine);
                }

                foreach (var item in Config.Instance.Data.alreadySerached)
                {
                    _instartAutoHeart.tbExceptTags.AppendText(item);
                    _instartAutoHeart.tbExceptTags.AppendText(System.Environment.NewLine);
                }

                foreach (var item in Config.Instance.Data.exceptStrings)
                {
                    _instartAutoHeart.tbExceptStr.AppendText(item);
                    _instartAutoHeart.tbExceptStr.AppendText(System.Environment.NewLine);
                }

                bool isHideChrome = false;
#if _DEBUG
                isHideChrome = false;
#endif

                // Init Selenium
                _driverService = ChromeDriverService.CreateDefaultService();
                _driverService.HideCommandPromptWindow = isHideChrome;

                _options = new ChromeOptions();
                if(_driverService.HideCommandPromptWindow)
                {
                    _options.AddArgument("disable-gpu");
                    _options.AddArgument("headless"); // 창을 숨기는 옵션입니다.
                }
            }
            catch (Exception e)
            {
                _instartAutoHeart.SendLog("Manager init fail... " + e.Message.ToString());
                return;
            }

            _instartAutoHeart.isInit = true;
        }

        public bool Login(string ID, string PW)
        {
            if (!_instartAutoHeart.isInit)
                return false;

            try
            {
                KillOpenChromeDrivers();

                _driver = new ChromeDriver(_driverService, _options);
                _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 60));

                if (!Config.Instance.Data.dailyLoginCount.ContainsKey(DateTime.Today.ToString()))
                    Config.Instance.Data.dailyLoginCount.Add(DateTime.Today.ToString(), 0);

#if !_DEBUG
                if (Config.Instance.Data.dailyLoginCount[DateTime.Today.ToString()] >= 5)
                {
                    _instartAutoHeart.SendLog("당일 로그인 카운트 n번 이상이어서 실패 처리(위험)");
                    return false;
                }
#endif
                // login
                    _driver.Navigate().GoToUrl("https://www.instagram.com/?hl=ko");

                WaitForVisivle(By.XPath("//*[@id=\"loginForm\"]/div/div[2]/div/label/input"));

                var id = _driver.FindElementByXPath("//*[@id=\"loginForm\"]/div/div[1]/div/label/input");
                id.SendKeys(ID);

                System.Threading.Thread.Sleep(1000);
                var pw = _driver.FindElementByXPath("//*[@id=\"loginForm\"]/div/div[2]/div/label/input");
                pw.SendKeys(PW);

                System.Threading.Thread.Sleep(1000);
                var loginBtn = _driver.FindElementByXPath("//*[@id=\"loginForm\"]/div/div[3]/button/div");

                System.Threading.Thread.Sleep(1500);
                loginBtn.Click();
                System.Threading.Thread.Sleep(10000);


                // 로그인 후 쿠키 저장 다음에 하기 버튼
                WaitForVisivle(By.XPath("/html/body/div[1]/section/main/div/div/div/div/button")).Click();

                // 알림 설정 안함
                WaitForVisivle(By.XPath("/html/body/div[4]/div/div/div/div[3]/button[2]")).Click();

                // 검색 인풋창이 보이면 로그인 성공
                WaitForVisivle(By.XPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/input"));

            }
            catch (Exception e)
            {
                _instartAutoHeart.SendLog("thow exception : " + e.Message.ToString() + ", trace : " + e.StackTrace.ToString());
                return false;
            }

            Config.Instance.Data.dailyLoginCount[DateTime.Today.ToString()]++;

            return true;
        }

        public void KillOpenChromeDrivers()
        {
            Process[] killChrome = Process.GetProcessesByName("chromedriver.exe");

            foreach (var process in killChrome)
            {
                process.Kill();
            }
        }


        private IWebElement WaitForVisivle(By by)
        {
            try
            {
                IWebElement element = _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
                return element;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void WorkStop()
        {
            isIntercept = true;
            _worker.Interrupt();
            _worker.Abort();
        }

        public void WorkStart()
        {
            isIntercept = false;
            _worker = new Thread(Worker);

            _worker.Start();
        }

        public void Worker()
        {
            try
            {
                worketState = WorkerState.SettingKeyword;

                while (true)
                {
                    UpdateWorker();

                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception e)
            {
                _instartAutoHeart.SendLog("치명적인 오류가 발생하여 드라이버 초기화, 1시간대기 -> 작업 재개 시도");
                _instartAutoHeart.SendLog("throw E : " + e.Message.ToString() + ", trace : " + e.StackTrace.ToString());            
                return;
            }
            finally
            {
                if (!isIntercept)
                {
                    if (_driver != null)
                        _driver.Quit();

                    _driver = null;

                    System.Threading.Thread.Sleep(3600);

                    if(Login(_instartAutoHeart.tbID.Text, _instartAutoHeart.tbPW.Text))
                    {
                        WorkStart();
                    }
                }
            }
        }

        public void UpdateWorker()
        {
            switch (worketState)
            {
                case WorkerState.None:
                    {
                        worketState = WorkerState.CollectingTag;
                        _currentTarget = new TargetData();
                    }
                    break;
                case WorkerState.SettingKeyword:
                    {
                        if(_searchWords.Count == 0) // 찾을 글자가 없으면 세팅
                        {
                            foreach (var item in Config.Instance.Data.priorityTags)
                            {
                                _searchWords.Enqueue("#" + item);
                            }

                            foreach (var item in Config.Instance.kors)
                            {
                                _searchWords.Enqueue("#" + item);
                            }

                            _instartAutoHeart.SendLog("키워드 데이터 삽입 완료 : " + _searchWords.Count.ToString() + "개");
                        }

                        worketState = WorkerState.CollectingTag;
                    }
                    break;
                case WorkerState.CollectingTag:
                    {
                        System.Threading.Thread.Sleep(5000);
                        // https://www.instagram.com/
                        _driver.Navigate().GoToUrl("https://www.instagram.com");
                        
                        // 인풋창 대기
                        var input = WaitForVisivle(By.XPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/input"));
                        input.Clear();
                        if (_currentTarget == null || _currentTarget.tags.Count == 0)
                        {
                            _currentTarget = new TargetData();

                            var targetKey = _searchWords.Dequeue();

                            // 테그 긁어오기
                            _currentTarget.searchKeyword = targetKey;

                            input.SendKeys(_currentTarget.searchKeyword);
                            List<string> tempList = new List<string>();
                            var list = WaitForVisivle(By.XPath("/html/body/div[1]/section/nav/div[2]/div/div/div[2]/div[3]/div/div[2]/div"));

                            bool isSave = false;
                            string buffer = "";
                            for (int i = 0; i < list.Text.Count(); i++)
                            {
                                if (!isSave && list.Text[i] != '#')
                                    continue;
                                else if (list.Text[i] == '#')
                                    isSave = true;
                                else if (list.Text[i] == '\r')
                                {
                                    if (buffer != "")
                                    {
                                        if(Config.Instance.Data.alreadySerached.Exists(w => w == buffer))
                                        {
                                            _instartAutoHeart.SendLog("제외 테그 버림 : " + buffer);
                                        }
                                        else
                                        {
                                            tempList.Add(buffer);
                                            _instartAutoHeart.SendLog("tag 수집 : " + buffer);
#if _DEBUG
                                            break;
#endif
                                            if (tempList.Count >= 10)
                                                break;
                                        }
                                    }

                                    buffer = "";

                                    isSave = false;
                                }
                                    
                                if(isSave)
                                {
                                    buffer += list.Text[i];
                                }
                            }

                            //tempList.Shuffle();
                            foreach (var item in tempList)
                            {
                                _currentTarget.tags.Enqueue(item);
                            }

                            _instartAutoHeart.SendLog(targetKey + ": Tag 수집 완료");
                        }


                        worketState = WorkerState.WorkHeartChecking;
                    }
                    break;
                case WorkerState.WorkHeartChecking:
                    {
                        if(_currentTarget.tags.Count == 0)
                        {
                            worketState = WorkerState.SettingKeyword;
                            _instartAutoHeart.SendLog("대상 태그 목록 작업 완료 재 세팅 진행");
                            return;
                        }

                        //아래 주소로 이동
                        var k = _currentTarget.tags.Dequeue();
                        //Config.Instance.Data.alreadySerached.Add(k);

                        Config.Instance.Save();
                        _instartAutoHeart.SetCurrentTargetTag(k);

                        string targetKey = k.Replace("#", "");
                        string log = string.Format("{0} 좋아요 작업 진행중, 남은 tags : {1}, 남은 keywords : {2}", targetKey, _currentTarget.tags.Count, _searchWords.Count);
                        _instartAutoHeart.SendLog(log);

                        string url = string.Format("https://www.instagram.com/explore/tags/{0}/", targetKey);
                        _driver.Navigate().GoToUrl(url);
                        var pic = WaitForVisivle(By.XPath("/html/body/div[1]/section/main/article/div[1]/div/div/div[1]/div[1]/a/div"));
                        pic.Click();

                        bool first = false;
                        int cnt = 0;
                        int totalCnt = 50;
                        Random random = new Random();
                        int dailyHeartCount = random.Next(850, 940);
                        while (true)
                        {
                            // 걍 한 테그 n개 이상 좋아요 누르면 넘어가도록(테그가 넘많다)
                            if (cnt++ > totalCnt)
                                break;

                            if (!Config.Instance.Data.dailyHeartCount.ContainsKey(DateTime.Today.ToString()))
                                Config.Instance.Data.dailyHeartCount.Add(DateTime.Today.ToString(), 0);

                            while (Config.Instance.Data.dailyHeartCount[DateTime.Today.ToString()] >= dailyHeartCount)
                            {
                                _instartAutoHeart.SendLog("금일 좋아요 가능 개수 초과로 4시간 대기 시작..");
                                System.Threading.Thread.Sleep(1000 * 3600 * 4);
                            }

                            System.Threading.Thread.Sleep(random.Next(1000, 2000));

                            var pageLoad = WaitForVisivle(By.XPath("/html/body/div[5]/div[2]/div/article/header"));
                         
                            var heart = _driver.FindElementsByXPath("/html/body/div[5]/div[2]/div/article/div[3]/section[1]/span[1]/button/div/span");
                            if (heart.Count != 0)
                            {
                                heart[0].Click();
                                Config.Instance.Data.dailyHeartCount[DateTime.Today.ToString()]++;
                                Config.Instance.Save();
                            }
                            else
                                break;

                            System.Threading.Thread.Sleep(1000);

                            // 확률 적 팔로잉 시도
                            if (!Config.Instance.Data.dailyFollowCount.ContainsKey(DateTime.Today.ToString()))
                                Config.Instance.Data.dailyFollowCount.Add(DateTime.Today.ToString(), 0);

                            // 팔로우 안하도록
                            //if (Config.Instance.Data.dailyFollowCount[DateTime.Today.ToString()] <= 200)
                            if (false)
                            {
                                if(random.Next(1, 100) <= 1)
                                {
                                    // 팔로잉 ㄱㄱ
                                    var follow = _driver.FindElementsByXPath("/html/body/div[4]/div[2]/div/article/header/div[2]/div[1]/div[2]/button");
                                    if(follow.Count != 0)
                                    {
                                        _instartAutoHeart.SendLog("팔로잉 이벤트 발생! 금일 팔로잉 수 : " + Config.Instance.Data.dailyFollowCount[DateTime.Today.ToString()].ToString() + " 현재 남은 tag : " + _currentTarget.tags.Count().ToString());
                                        follow[0].Click();
                                        Config.Instance.Data.dailyFollowCount[DateTime.Today.ToString()]++;
                                        Config.Instance.Save();
                                    }
                                }
                            }

                            System.Threading.Thread.Sleep(random.Next(30000, 48000));
                            _instartAutoHeart.SendLog(string.Format("진행중.. {0} / {1}, 금일 좋아요 개수 : {2}", cnt, totalCnt, Config.Instance.Data.dailyHeartCount[DateTime.Today.ToString()]));

                            var next = _driver.FindElementsByXPath("/html/body/div[5]/div[1]/div/div/a");
                            if (next.Count == 1 && first)
                                break;

                            var next2 = _driver.FindElementsByXPath("/html/body/div[5]/div[1]/div/div/a[2]");
                            if(next2.Count != 0)
                            {
                                next2[0].Click();
                                continue;
                            }

                            if (next.Count == 0)
                                break;

                            next[0].Click();

                            first = true;
                        }
                    }
                    break;
            }
        }
    }
}
