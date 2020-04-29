#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace XmlComparer
{
    public static class Comparer
    {
        public static string CompareXmls(string xml1, string xml2)
        {
            XmlDocument xdoc1 = new XmlDocument();
            xdoc1.LoadXml(xml1);

            XmlDocument xdoc2 = new XmlDocument();
            xdoc2.LoadXml(xml2);
            MyXmlNode xroot1 = new MyXmlNode(xdoc1.DocumentElement);
            MyXmlNode xroot2 = new MyXmlNode(xdoc2.DocumentElement);
            string compareLog = CompareNodes(xroot1, xroot2, "");
            return compareLog;
        }

        public static string CompareNodes(MyXmlNode xroot1, MyXmlNode xroot2, string tabprefix)
        {
            string result = "";
            for (int i = 0; i < xroot1.xnode.ChildNodes.Count; i++)
            {
                XmlNode xnode1 = xroot1.xnode.ChildNodes[i];
                string xnode1name = xnode1.Name;
                XmlNode xnode2 = xroot2.xnode[xnode1name];
                if(xnode2 == null)
                {
                    result += "\nDeleted element " + xnode1name;
                }

                string curfieldName = xnode1.Name;

                if (xnode1.Name.StartsWith("List_"))
                {
                    string curprefix = tabprefix + "\t";
                    int count1 = xnode1.ChildNodes.Count;
                    int count2 = xnode2.ChildNodes.Count;


                    List<MyXmlNode> elems1 = MyXmlNode.GetNodesList(xnode1);
                    List<MyXmlNode> elems2 = MyXmlNode.GetNodesList(xnode2);

                    List<MyXmlNode> removedElems = elems1.Except(elems2).ToList();
                    List<MyXmlNode> newElems = elems2.Except(elems1).ToList();

                    List<MyXmlNode> commonElems = elems1.Intersect(elems2).ToList();

                    string listResult = "";
                    if (commonElems.Count > 0 && commonElems[0].IsComplexType == true)
                    {
                        for (int j = 0; j < commonElems.Count; j++)
                        {
                            MyXmlNode listElem1 = elems1[j];
                            string listElem1Id = listElem1.Id;
                            List<MyXmlNode> nodes2 = elems2.Where(n => n.Id == listElem1Id).ToList();
                            if(nodes2.Count == 0)
                            {
                                listResult += "\n" + "Deleted element " + listElem1.xnode.Name + " id " + listElem1Id;
                                continue;
                            }
                            MyXmlNode listElem2 = nodes2.First();
                            listResult += CompareNodes(listElem1, listElem2, curprefix);
                        }
                    }

                    if ((count1 != count2) || listResult != "" || removedElems.Count > 0 || newElems.Count > 0)
                    {
                        result += "\n" + curprefix + curfieldName + " is changed!!" + "\n";

                        string listtabprefix = curprefix + "\t";
                        if (count1 != count2)
                        {
                            result += listtabprefix + "count is ";
                            if (count1 > count2)
                                result += " increased from " + count1 + " to " + count2 + "\n";
                            else
                                result += " decreased from " + count1 + " to " + count2 + "\n";
                        }

                        if (removedElems.Count > 0 || newElems.Count > 0)
                        {
                            result += VisualizeList("deleted", removedElems, listtabprefix);
                            result += VisualizeList("new", newElems, "  " + listtabprefix);
                        }

                        if (listResult != "")
                        {
                            result += listResult;
                        }
                    }
                }
                else
                {
                    string val1 = xnode1.InnerText;
                    string val2 = xnode2.InnerText;
                    if (val1 != val2)
                        result += "\n\t" + tabprefix + curfieldName + " is changed: " + val1 + " -> " + val2;
                }
            }

            if (result != "")
                result = tabprefix + xroot1.xnode.Name + " " + xroot1.Id + result + "\n";
            return result;
        }



        private static string VisualizeList(string title, List<MyXmlNode> elems, string prefix)
        {
            if (elems.Count == 0) return "";
            string result = "";
            result += prefix + title + "\n";
            prefix += "  ";
            foreach (MyXmlNode e in elems)
            {
                result += prefix + e.Id + "\n";
            }
            return result;
        }

    }
}
