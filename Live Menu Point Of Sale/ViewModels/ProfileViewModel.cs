using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Live_Menu_Point_Of_Sale.ViewModels
{
    public class ProfileViewModel : Screen
    {
        public string CreationTime { get; set; }

        public ProfileViewModel()
        {
            CreationTime = DateTime.Now.ToString();

        }
    }
}
