using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using VIREO_KIS.Properties;
using WpfControls.Editors;

namespace VIREO_KIS
{
    class ConceptSuggestionProvider : ISuggestionProvider
    {
        public System.Collections.IEnumerable GetSuggestions(string filter)
        {            
            if (string.IsNullOrEmpty(filter))
            {
                return null;
            }
            filter = filter.ToLower();

            string[] listConcept = File.ReadAllLines(ConceptSearchResult.LNK_TO_LIST);
            //string[] listConcept = new string[0];
            List<string> toRecommend = new List<string>();
            for (int i = 0; i < listConcept.Length; i++)
            {
                listConcept[i] = listConcept[i].Trim();
                if (listConcept[i].StartsWith(filter))
                {
                    toRecommend.Add(listConcept[i]);
                }
            }

            toRecommend.Sort();
            return toRecommend;
        }
    }
}
