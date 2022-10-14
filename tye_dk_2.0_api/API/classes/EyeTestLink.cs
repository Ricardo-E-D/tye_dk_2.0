// copyright (c) 2013 by monosolutions (Michael 'mital' H. Pedersen / mital.dk)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tye.Data {
    public class EyeTestLink
    {

        public EyeTestLink() { }

        public int ID { get; set; }
        public string LinkUrl { get; set; }
        public string LinkName { get; set; }
        public int OpticianID { get; set; }
        public int EyeTestID { get; set; }

    }
}
