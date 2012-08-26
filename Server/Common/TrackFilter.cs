﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CrisisTracker.Common
{
    public class TrackFilter
    {
        public enum FilterType { Undefined = -1, Word = 0, User = 1, Region = 2 }

        public int? ID { get; set; }
        public bool? IsStrong { get; set; }
        public FilterType Type { get; set; }
        private string _word;
        public string Word
        {
            get
            {
                return _word;
            }
            set
            {
                _word = value;
                _stemmedWord = WordCount.NaiveStemming(_word);
            }
        }
        private string _stemmedWord;
        public long? UserID { get; set; }
        public double? Longitude1 { get; set; }
        public double? Longitude2 { get; set; }
        public double? Latitude1 { get; set; }
        public double? Latitude2 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is TrackFilter)
                return ID.Equals(((TrackFilter)obj).ID);
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }

        public override string ToString()
        {
            switch (Type)
            {
                case FilterType.Word:
                    return Word;
                case FilterType.User:
                    return UserID.ToString();
                case FilterType.Region:
                    return Longitude1 + "," + Latitude1 + "," + Longitude2 + "," + Latitude2;
                default:
                    return "";
            }
        }

        public bool Match(string[] words, long userID, double? longitude, double? latitude, bool useStemming=true)
        {
            switch (Type)
            {
                case FilterType.Word:
                    if (words != null 
                        && words.Length > 0 
                        && Word != null 
                        && (useStemming && words.Contains(_stemmedWord)) || (!useStemming && words.Contains(_word)))
                        return true;
                    return false;
                case FilterType.User:
                    if (userID == UserID)
                        return true;
                    return false;
                case FilterType.Region:
                    if (longitude.HasValue && latitude.HasValue
                        && longitude > Longitude1 && longitude < Longitude2
                        && latitude > Latitude1 && latitude < Latitude2)
                        return true;
                    return false;
                default:
                    return false;
            }
        }
    }
}