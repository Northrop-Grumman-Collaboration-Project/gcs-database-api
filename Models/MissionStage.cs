using System.Text.Json;

namespace Database.Models
{
    public class MissionStage
    {
        public string key { get; set; }
        public string stageId { get; set; }
        public string stageName { get; set; }
        public string stageStatus { get; set; }
        private Stage_Enum MissionStageStatus { get; set; }

        public MissionStage(string key, string stageId, string stageName, string stageStatus)
        {
            this.key = key;
            this.stageId = stageId;
            this.stageName = stageName;
            this.stageStatus = stageStatus;

            string lowerCaseStatus = stageStatus.ToLower();

            if (lowerCaseStatus == "not_started")
            {
                this.MissionStageStatus = Stage_Enum.NOT_STARTED;
            }
            else if (lowerCaseStatus == "initialized")
            {
                this.MissionStageStatus = Stage_Enum.INITIALIZED;
            }
            else if (lowerCaseStatus == "in_progress")
            {
                this.MissionStageStatus = Stage_Enum.IN_PROGRESS;
            }
            else if (lowerCaseStatus == "completed")
            {
                this.MissionStageStatus = Stage_Enum.COMPLETE;
            }
            else
            {
                this.MissionStageStatus = Stage_Enum.UNKNOWN;
            }

        }

        public override string ToString()
        {
            var objectToSerialize = new
            {
                key = this.key,
                stageId = this.stageId,
                stageName = this.stageName,
                stageStatus = this.MissionStageStatus
            };

            return JsonSerializer.Serialize(objectToSerialize);
        }
    }
}