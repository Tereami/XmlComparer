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
using System.Xml;

namespace XmlComparer
{
    public class MyXmlNode
    {
        public XmlNode xnode;
        public string Id;
        public bool IsComplexType;

        public MyXmlNode(XmlNode node)
        {
            xnode = node;

            XmlNode idnode = node["Name"];
            if (idnode == null)
            {
                idnode = node["Id"];
                if (idnode == null)
                {
                    IsComplexType = false;
                    Id = node.InnerText;
                    return;
                }
            }
            IsComplexType = true;
            Id = idnode.InnerText;
        }

        public bool Equals(MyXmlNode other)
        {
            if (other is null) return false;
            return this.Id == other.Id;
        }

        public override bool Equals(object obj) => Equals(obj as MyXmlNode);
        public override int GetHashCode() => (Id).GetHashCode();

        public static List<MyXmlNode> GetNodesList(XmlNode node)
        {
            List<MyXmlNode> list = new List<MyXmlNode>();
            for (int i = 0; i < node.ChildNodes.Count; i++)
            {
                MyXmlNode mn = new MyXmlNode(node.ChildNodes[i]);
                list.Add(mn);
            }
            return list;
        }
    }
}
