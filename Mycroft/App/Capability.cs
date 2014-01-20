using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// An capability that represents a dependency between apps
    /// </summary>
    public class Capability : IComparable
    {
        /// <summary>
        /// The name of the capability
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The version of the capability
        /// </summary>
        public Version Version { get; private set; }

        public Capability(string name, Version version)
        {
            this.Name = name;
            this.Version = version;
        }

        /// <summary>
        /// Checks that capabilitiees with the same name and version are the same
        /// </summary>
        /// <param name="obj">The object being compared</param>
        /// <returns>Returns true if the name and version match, false otherwise</returns>
        public override bool Equals(object obj)
        {
            var other = obj as Capability;
            return (other != null) && (other.Name == Name) && (other.Version.Equals(Version));
        }

        /// <summary>
        /// Compares capabilities based on name, then version
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        int IComparable.CompareTo(object other)
        {
            var capB = other as Capability;
            if (capB == null) return 1;

            var nameCompare = Name.CompareTo(capB.Name);
            var versionCompare = Version.CompareTo(capB.Version);

            if (nameCompare == 0)
            {
                return versionCompare;
            }
            return nameCompare;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Version.GetHashCode();
        }
    }
}
