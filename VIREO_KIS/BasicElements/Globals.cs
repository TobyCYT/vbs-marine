using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIREO_KIS
{
    public static class Globals //doing some dangerous thing here so be sure not to accidentally modify the values
    {
        public static Hashtable vid2idx = new Hashtable();
        public static Hashtable idx2vid = new Hashtable();
        public static Hashtable marine2trec = new Hashtable();
        public static Hashtable trec2marine = new Hashtable();
    }
}
