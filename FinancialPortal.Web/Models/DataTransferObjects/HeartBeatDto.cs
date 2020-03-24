using System.Collections.Generic;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class HeartBeatDto : ServiceStatusHeartBeatResult
    {
        public List<ServiceStatusHeartBeatResult> ServiceStatusHeartBeatResults { get; set; }

        private string _overallStatus = null;

        public string OverallStatus {
            get => _overallStatus;
            set
            {
                switch (value)
                {
                    case "GREEN":
                        SolarWindsOverallStatus = 1;
                        break;
                    case "RED":
                        SolarWindsOverallStatus = 3;
                        break;
                    case "AMBER":
                        SolarWindsOverallStatus = 2;
                        break;
                }

                _overallStatus = value;
            }
        }
        public int SolarWindsOverallStatus { get; set; }
        public string OverallStatusHeader {
            get
            {
                switch (OverallStatus)
                {
                    case "GREEN":
                        return "notification--notify";
                    case "RED":
                        return "notification--error";
                    case "AMBER":
                        return "notification--warning";
                    default:
                        return string.Empty;
                }
            }
        }

        public HeartBeatDto()
        {
            ServiceStatusHeartBeatResults = new List<ServiceStatusHeartBeatResult>();
        }

        public void SetStatus(int greenThreshold, int redThreshold)
        {
            if (RunningElapsedTime.TotalSeconds < greenThreshold || RunningElapsedTime.TotalMilliseconds <= 1)
            {
                Status = "GREEN";
                return;
            }

            Status = RunningElapsedTime.TotalSeconds > redThreshold ? "RED" : "AMBER";
        }
    }
}