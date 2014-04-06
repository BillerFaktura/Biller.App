using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Biller.Data.Articles
{
    public class Article : Utils.PropertyChangedHelper, Interfaces.IXMLStorageable
    {
        public Article()
        {
            // insert empty strings to avoid null exceptions.
            ArticleID = ""; ArticleDescription = ""; ArticleText = "";
            TaxClass = new Utils.TaxClass(); ArticleUnit = new Utils.Unit();
            ArticleCategory = ""; Price1 = new Interfaces.PriceModel(this); Price2 = new Interfaces.PriceModel(this); Price3 = new Interfaces.PriceModel(this);
        }

        /// <summary>
        /// Unique identifier of the article. The article can be found in the database with this string.
        /// </summary>
        public string ArticleID
        {
            get { return GetValue(() => ArticleID); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the name of the article.
        /// </summary>
        public string ArticleDescription
        {
            get { return GetValue(() => ArticleDescription); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a short extra text to describe the article.
        /// </summary>
        public string ArticleText
        {
            get { return GetValue(() => ArticleText); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Determines to which <see cref="TaxClass"/> the article belongs.
        /// </summary>
        public Utils.TaxClass TaxClass
        {
            get { return GetValue(() => TaxClass); }
            set { SetValue(value); }
        }
        
        /// <summary>
        /// Represents the weight in kilogramm per <see cref="ArticleUnit"/>.
        /// </summary>
        public double ArticleWeight
        {
            get { return GetValue(() => ArticleWeight); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the unit the article will be saled with.
        /// </summary>
        public Utils.Unit ArticleUnit
        {
            get { return GetValue(() => ArticleUnit); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents a string to identify the category this article belongs to.\n
        /// See the <see cref="Category"/> class to see how this string is build.
        /// </summary>
        public string ArticleCategory
        {
            get { return GetValue(() => ArticleCategory); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the price of pricegroup 1.
        /// </summary>
        public Interfaces.PriceModel Price1
        {
            get { return GetValue(() => Price1); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the price of pricegroup 2.
        /// </summary>
        public Interfaces.PriceModel Price2
        {
            get { return GetValue(() => Price2); }
            set { SetValue(value); }
        }

        /// <summary>
        /// Represents the price of pricegroup 3.
        /// </summary>
        public Interfaces.PriceModel Price3
        {
            get { return GetValue(() => Price3); }
            set { SetValue(value); }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is Article)
            {
                if ((obj as Article).ArticleID == this.ArticleID)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override string ToString()
        {
            return "{Article: ID=" + this.ArticleID + " Name=" + this.ArticleDescription + "}";
        }

        /// <summary>
        /// <inheritdoc/>
        /// <remarks>The <see cref="TaxClass"/> and <see cref="ArticleUnit"/> are just saved by their names!</remarks>
        /// </summary>
        /// <returns></returns>
        public System.Xml.Linq.XElement GetXElement()
        {
            var element = new XElement(XElementName);
            element.Add(new XElement("ArticleID", Utils.XML.Fix(ArticleID)), new XElement("ArticleDescription", Utils.XML.Fix(ArticleDescription)), new XElement("ArticleText", Utils.XML.Fix(ArticleText)),
                new XElement("TaxClass", Utils.XML.Fix(TaxClass.Name)), new XElement("ArticleWeight", Utils.XML.Fix(ArticleWeight)), new XElement("ArticleUnit", Utils.XML.Fix(ArticleUnit.Name)),
                new XElement("ArticleCategory", ArticleCategory), new XElement("Price1", Price1.GetXElement()),
                new XElement("Price2", Price2.GetXElement()), new XElement("Price3", Price3.GetXElement()));
            return element;
        }

        /// <summary>
        /// Parses the articles properties to read an object of a database.\n
        /// <b>ATTENTION: Does not parse <see cref="TaxClass"/> and <see cref="ArticleUnit"/>! <see cref="GetXElement"/>'s output references this properties just with their IDs (<see cref="TaxClass.Name"/> and <see cref="ArticleUnit.Name"/>).</b>
        /// </summary>
        /// <param name="source"></param>
        public void ParseFromXElement(System.Xml.Linq.XElement source)
        {
            if (source.Name != XElementName)
                throw new Exception("Name of XElement was " + source.Name + " but expected " + XElementName);

            ArticleID = source.Element("ArticleID").Value;
            ArticleDescription = source.Element("ArticleDescription").Value;
            ArticleText = source.Element("ArticleText").Value;
            ArticleWeight = double.Parse(source.Element("ArticleWeight").Value);
            ArticleCategory = source.Element("ArticleCategory").Value;
            Price1.ParseFromXElement(source.Element("Price1").Element(Price1.XElementName));
            Price2.ParseFromXElement(source.Element("Price2").Element(Price2.XElementName));
            Price3.ParseFromXElement(source.Element("Price3").Element(Price3.XElementName));
        }

        /// <summary>
        /// Identifier string that is used by <see cref="ParseFromXElement"/> and <see cref="GetXElement"/> to define the XElement's name.
        /// </summary>
        public string XElementName
        {
            get { return "Article"; }
        }

        /// <summary>
        /// Returns <see cref="ArticleID"/>
        /// </summary>
        public string ID
        {
            get { return ArticleID; }
        }

        /// <summary>
        /// Returns a new <see cref="Article"/>.
        /// </summary>
        /// <returns></returns>
        public Interfaces.IXMLStorageable GetNewInstance()
        {
            return new Article();
        }

        /// <summary>
        /// Returns "ArticleID".
        /// </summary>
        public string IDFieldName
        {
            get { return "ArticleID"; }
        }
    }
}