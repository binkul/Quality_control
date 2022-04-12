using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quality_Control.Forms.Quality.ModelView
{
    class QualityDataMV : QualityMV
    {

        public DataView QualityData => Service.DataQualityView;
    }
}
