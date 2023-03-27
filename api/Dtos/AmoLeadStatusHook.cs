using System.Collections.Generic;

namespace AmoToSheetFunc
{
    public class AmoLeadStatusHook
    {
        public long LeadId { get; set; }
        public long NewStatusId { get; set; }
        public long NewPipelineId { get; set; }
        public long OldStatusId { get; set; }
        public long OldPipelineId { get; set; }
    }

}