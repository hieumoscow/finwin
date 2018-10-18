using System;
using System.Collections.Generic;
using System.Text;

namespace Finwin.Common.Models
{
    /// <summary>
    /// 
    /// </summary>
    public enum Topic
    {
        News,
        Profile,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Subject
    {
        Finance,
        Business,
    }

    /// <summary>
    /// 
    /// </summary>
    public class Query
    {
        public Topic Topic;

        public string Company;

        public string Subject;
    }
}
