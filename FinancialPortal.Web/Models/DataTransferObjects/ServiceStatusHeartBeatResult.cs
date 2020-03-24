using System;

namespace FinancialPortal.Web.Models.DataTransferObjects
{
    public class ServiceStatusHeartBeatResult
    {
        public string ServiceName { get; set; }
        public string Details { get;set; }
        public TimeSpan RunningElapsedTime { get; set; }
        public TimeSpan TotalElapsedTime { get; set; }
        public string FriendlyDisplayElapsedTime => $"{TotalElapsedTime.Hours:00}:{TotalElapsedTime.Minutes:00}:{TotalElapsedTime.Seconds:00}.{TotalElapsedTime.Milliseconds / 10:00}";
        public string Status { get; set; }
        public HeartBeatDto ChildHeartBeat { get; set; }

        public string HeaderColour
        {
            get
            {
                switch (Status)
                {
                        case "GREEN":
                            return "info-box--notify";
                        case "RED":
                            return "info-box--error";
                        case "AMBER":
                            return "info-box--warning";
                        default:
                            return string.Empty;
                }
            }
        }
    }
}