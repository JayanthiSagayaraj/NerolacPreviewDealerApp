using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NerolacPreviewApp.Models
{
    public class ColorChoices
    {
        public int ProjectID { get; set; }

        public string ProjectName { get; set; }

        public string NeroliteID { get; set; }

        public string NeroliteName { get; set; }

        public string NeroliteNo { get; set; }
        public string NeroliteEmail { get; set; }

        public string Body1 { get; set; }

        public string Border1 { get; set; }

        public string Highlight1 { get; set; }
        public string Body2 { get; set; }

        public string Border2 { get; set; }

        public string Highlight2 { get; set; }

        public string Body3 { get; set; }

        public string Border3 { get; set; }
        public string Highlight3 { get; set; }
        public string Options { get; set; }
        public string Resolution { get; set; }

        public string Size { get; set; }

        public string caseID { get; set; }

        public string SpecialRequest { get; set; }
        public string InsyComments { get; set; }

        public string hr1 { get; set; }
        public string hg1 { get; set; }
        public string hb1 { get; set; }
        public string hr2 { get; set; }
        public string hg2 { get; set; }
        public string hb2 { get; set; }
        public string hr3 { get; set; }
        public string hg3 { get; set; }
        public string hb3 { get; set; }
        public string snBody1 { get; set; }

        public string snBorder1 { get; set; }

        public string snhl1 { get; set; }
        public string snBody2 { get; set; }

        public string snBorder2 { get; set; }

        public string snhl2 { get; set; }
        public string snBody3 { get; set; }

        public string snBorder3 { get; set; }

        public string snhl3 { get; set; }


        public string remarks { get; set; }
        public List<ColorsUsed> coloursused { get; set; }
    }
    

    }