using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReadyEDI.EntityFactory
{
    [Serializable]
    public class EntityException : Exception
    {
        private int _type = 0;
        private DateTime _throwTime = DateTime.Now;

        public EntityException()
            : base()
        {

        }

        public EntityException(int type, string message, System.Exception ex)
            : base(ex)
        {
            _type = type;
        }

        public int Type
        {
            get { return _type; }
        }

        private void Garnish(string trace)
        {
            trace.Replace("Line ", "<span style='color:#FF0000;'><b>line </b></span>")
                .Replace(" at ", "<br />at ");
        }

        private string GetPadding(int tabs)
        {
            string ret = String.Empty;

            int spaces = tabs * 3;

            for (int i = 0; i < spaces; i++)
            {
                ret += "&nbsp;";
            }

            return ret;
        }

        public string Print(Exception ex)
        {
            return string.Format(@"
<div style='font-size:1.4em;color:#FF0000;margin-bottom:10px;'>
<i>{0}: {1}</i>
</div>
{2}
<div style='margin:10px;padding:5px;background-color:#FFFFCC;'>
{3}
</div>
", ex.Source, ex.Message, _throwTime, ex.StackTrace ?? ""
                .Replace("Line ", "<span style='color:#FF0000;'><b>line </b></span>")
                .Replace(" at ", "<br />at "));
        }

        private void GenerateState(IEntity entity, StringBuilder sb, int tabs = 0)
        {
            entity.__Elements.ForEach(e =>
            {
                if (e.IsEntity)
                {
                    sb.Append(string.Format(@"
<div>
<div style='float:left;width:180px;background-color:#FFFFCC;'><i>{0}</i></div>
<div style='margin-left:200px;'>&nbsp;</div>
</div>
", String.Concat(GetPadding(tabs), e.Name)));

                    GenerateState(e.Data as IEntity, sb, tabs + 1);
                }
                else
                    sb.Append(string.Format(@"
<div>
<div style='float:left;width:180px;background-color:#FFFFCC;'>{0}: </div>
<div style='margin-left:200px;'>{1}</div>
</div>
", String.Concat(GetPadding(tabs), e.Name), e.Data == null || e.Data.ToString() == String.Empty ? "&nbsp;" : e.Data.ToString()));
            });
        }

        public string Print(IEntity entity)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"
<html>
<head>
</head>
<body style='margin: 0px 15px 0px 0px;'>
<div style='width:100%;'>
<div style='width:120px;height:100%;float:left;background-color:#CC6600;padding:20px 10px;font-size:1.2em;'>
<div><a id='Link_0' onclick='javascript:displayData(0)' style='cursor:pointer;color:#FFCC66;'><font onmouseover=""this.color='#FFCC66';"" onmouseout=""this.color='#FFCC66';"">State</font></a></div>
<div><a id='Link_1' onclick='javascript:displayData(1)' style='cursor:pointer;color:#FFFFFF;'><font onmouseover=""this.color='#993300';"" onmouseout=""this.color='#FFFFFF';"">Exception 1</font></a></div>
");

            int i = 2;
            Exception innerEx = InnerException;
            while (innerEx != null)
            {
                sb.Append(string.Format(@"
<div><a id='Link_{0}' onclick='javascript:displayData({0})' style='cursor:pointer;color:#FFFFFF;'><font onmouseover=""this.color='#993300';"" onmouseout=""this.color='#FFFFFF';"">Exception {0}</font></a></div>
", i.ToString()));
                i++;
                innerEx = innerEx.InnerException;
            }

            sb.Append(string.Format(@"
</div>
<div style='margin-left:150px;width:auto;'>
<div style='width:auto;font-size:1.6em;border-bottom:1px solid #000000;padding:15px;'><span>{0} Entity Exception</span></div>
<div style='margin:15px;'>
<div id='Data_0'>
", entity.__EntityName));

            GenerateState(entity, sb);

            sb.Append(string.Format("</div><div id='Data_1' style='display:none;'>{0}</div>", Print(this)));

            i = 2;
            innerEx = InnerException;
            while (innerEx != null)
            {
                sb.Append(string.Format(@"
</div>
<div id='Data_{0}' style='display:none;'>
{1}
</div>
", i.ToString(), Print(innerEx)));
                i++;
                innerEx = innerEx.InnerException;
            }

            sb.Append(@"
<script type='text/javascript'>
function displayData(index) {
for (var i=0;i<" + i + @";i++) {
document.getElementById('Data_' + i).style.display = 'none';
document.getElementById('Link_' + i).style.color = '#FFFFFF';
}
document.getElementById('Data_' + index).style.display = 'block';
document.getElementById('Link_' + index).style.color = '#FFCC66';
}
</script>
</body>
</html>
");

            return sb.ToString();
        }

        //public override void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    base.GetObjectData(info, context);
        //}

    }
}
