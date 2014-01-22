using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mycroft.App
{
    /// <summary>
    /// Represents a dependency. Unlike Mycroft.App.Capability this
    /// class supports a range of versions.
    /// 
    /// Ranges are specified by using a prefix before the version number,
    /// for example: >=1.2.3
    /// 
    /// Supported prefixes:
    /// 
    /// >=   : Any version greater than or equal to this version is supported
    /// >    : Any version greater than this version is supported
    /// <=   : Any version less than or equal to this version is supported
    /// <    : Any version less than this version is supported
    /// 
    /// No prefix indicates only the given version is supported
    /// </summary>
    class Dependency
    {
        public enum VersionRange { GreaterEqual, Greater, LessEqual, Less, Exact }

        public Capability InnerCapability { get; private set; }
        public VersionRange Range { get; private set; }

        /// <summary>
        /// Construct a new dependency.
        /// </summary>
        /// <param name="name">the name of the capability upon which this depends</param>
        /// <param name="version">a string of the version supplied</param>
        public Dependency(string name, string version)
        {
            if (version.StartsWith(">="))
                Range = VersionRange.GreaterEqual;
            else if (version.StartsWith(">"))
                Range = VersionRange.Greater;
            else if (version.StartsWith("<="))
                Range = VersionRange.LessEqual;
            else if (version.StartsWith("<"))
                Range = VersionRange.Less;
            else
                Range = VersionRange.Exact;

            if (Range == VersionRange.GreaterEqual || Range == VersionRange.LessEqual)
                version = version.Substring(2);
            else if (Range == VersionRange.Greater || Range == VersionRange.Less)
                version = version.Substring(1);

            InnerCapability = new Capability(name, new Version(version));
        }

        /// <summary>
        /// Determine whether the given capability is both the same name and
        /// within the acceptable range of versions for this dependency.
        /// </summary>
        /// <param name="other">The capability to which to compare.</param>
        /// <returns>
        ///     True only if the given capability is both the same name as this dependency
        ///     and within the range of acceptable versions.
        /// </returns>
        public bool Matches(Capability other)
        {
            if (InnerCapability.Name != other.Name)
                return false;

            var diff = other.CompareTo(InnerCapability);

            if (Range == VersionRange.Less)
                return diff < 0;
            if (Range == VersionRange.LessEqual)
                return diff <= 0;
            if (Range == VersionRange.Greater)
                return diff > 0;
            if (Range == VersionRange.GreaterEqual)
                return diff >= 0;
            if (Range == VersionRange.Exact)
                return diff == 0;

            throw new InvalidOperationException("Range was not found to match anything");
        }
    }
}
