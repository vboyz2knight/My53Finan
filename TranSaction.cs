using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml;

namespace MyWeb.CSharp.My53Finan
{

    public class TranSaction : IComparable<TranSaction>
    {
        private DateTime tranDate;
        private decimal tranAmnt;
        private string tranCategory;
        private string tranType;
        private string tranCheck;
        private string tranDescription;
        private string tranFilter;

        
        public int CompareTo(TranSaction other)
        {
            if (other.tranDate.Equals(this.tranDate))
            {
                return this.tranAmnt.CompareTo(other.tranAmnt);
            }
            return other.tranDate.CompareTo(this.tranDate);
        }


        public enum enCategory : short
        {
            Insurance, HomeBill, CarBill, Restaurant, Grocery, Other, Cable, PetBill, CommunicationBill, Bank
        }

        public string myFilter
        {
            get
            {
                return tranFilter;
            }
            set
            {
                tranFilter = value;
            }
        }

        public DateTime myDate
        {
            get
            {
                return tranDate;
            }
            set
            {
                tranDate = value;
            }
        }

        public decimal Amt
        {
            get
            {
                return tranAmnt;
            }
            set
            {
                tranAmnt = value;
            }
        }

        public string Description
        {
            get
            {
                return tranDescription;
            }
            set
            {
                tranDescription = value;
            }
        }

        public string myCategory
        {
            get
            {
                return tranCategory;
            }
            set
            {
                tranCategory = value;
            }
        }

        public string typeMy
        {
            get
            {
                return tranType;
            }
            set
            {
                tranType = value;
            }
        }

        public string check
        {
            get
            {
                return tranCheck;
            }
            set
            {
                tranCheck = value;
            }
        }


        public TranSaction(DateTime myDate, string myCategory, string myCheck, decimal myAmt, string myType, string mytranDescription,string mytranFilter)
        {
            tranDate = myDate;
            tranCategory = myCategory;
            tranCheck = myCheck;
            tranAmnt = myAmt;
            tranType = myType;
            tranDescription = mytranDescription;
            tranFilter = mytranFilter;
        }
    }
}