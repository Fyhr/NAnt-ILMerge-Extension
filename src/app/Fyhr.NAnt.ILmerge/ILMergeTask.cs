using System;
using System.Linq;
using NAnt.Core;
using NAnt.Core.Attributes;
using NAnt.Core.Types;
using NAnt.Core.Util;

namespace Fyhr.NAnt.ILMerge
{
	[TaskName("ilmerge")]
    public class ILMergeTask : Task
	{
		#region Fields

		private string _attributeFile;
        private string _excludeFile;
        private string _targetKind;
        private string _logFile;
        private string _outputFile;
        private string _keyFile;

		#endregion
		#region Properties

		[TaskAttribute("closed")]
		[BooleanValidator]
		public virtual bool Closed { get; set; }

		[TaskAttribute("copyattributes")]
		[BooleanValidator]
		public virtual bool CopyAttributes { get; set; }

		[TaskAttribute("debuginfo")]
		[BooleanValidator]
		public virtual bool DebugInfo { get; set; }

		[TaskAttribute("internalize")]
		[BooleanValidator]
		public virtual bool Internalize { get; set; }

		[BuildElement("librarypath")]
		public virtual FileSet LibraryPath { get; set; }

		[BuildElement("inputassemblies", Required = true)]
		public virtual FileSet InputAssemblies { get; set; }

		[TaskAttribute("attributefile")]
        public virtual string AttributeFile
        {
            get
            {
                return _attributeFile != null ? Project.GetFullPath(_attributeFile) : null;
            }
            set
            {
                _attributeFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

		[BuildElement("excludefile")]
        public virtual string ExcludeFile
        {
            get
            {
                return _excludeFile != null ? Project.GetFullPath(_excludeFile) : null;
            }
            set
            {
                _excludeFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("logfile")]
        public virtual string LogFile
        {
            get
            {
                return _logFile != null ? Project.GetFullPath(_logFile) : null;
            }
            set
            {
                _logFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("outputfile", Required = true)]
        [StringValidator(AllowEmpty = false)]
        public virtual string OutputFile
        {
            get
            {
                return _outputFile != null ? Project.GetFullPath(_outputFile) : null;
            }
            set
            {
                _outputFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("keyfile")]
        public virtual string KeyFile
        {
            get
            {
                return _keyFile != null ? Project.GetFullPath(_keyFile) : null;
            }
            set
            {
                _keyFile = StringUtils.ConvertEmptyToNull(value);
            }
        }

        [TaskAttribute("targetkind")]
        [StringValidator(AllowEmpty = false)]
        public virtual string TargetKind
        {
            get
            {
				return _targetKind ?? "sameasprimary";
            }
            set
            {
                _targetKind = StringUtils.ConvertEmptyToNull( value );
            }
        }

		#endregion
		#region Business Logic

		protected override void ExecuteTask()
        {
            if( InputAssemblies.FileNames.Count == 0 )
            {
                Log( Level.Error, "At least one assembly must be included" );
                return;
            }

			var merger = new ILMerging.ILMerge();

            merger.AttributeFile  = AttributeFile;
            merger.Closed         = Closed;
            merger.CopyAttributes = CopyAttributes;
            merger.DebugInfo      = DebugInfo;
            merger.ExcludeFile    = ExcludeFile;
            merger.Internalize    = Internalize;
            merger.LogFile        = LogFile;
            merger.Log            = !string.IsNullOrEmpty( _logFile );
            merger.OutputFile     = OutputFile;
            merger.KeyFile	      = KeyFile;

			merger.SetInputAssemblies( InputAssemblies.FileNames.Cast<string>().ToArray() );

			if( LibraryPath != null)
				merger.SetSearchDirectories( LibraryPath.FileNames.Cast<string>().ToArray() );

            switch( TargetKind.ToLower())
            {
                case "winexe":
                    merger.TargetKind = ILMerging.ILMerge.Kind.WinExe; break;
                case "exe":
                    merger.TargetKind = ILMerging.ILMerge.Kind.Exe; break;
                case "dll":
                    merger.TargetKind = ILMerging.ILMerge.Kind.Dll; break;
                case "sameasprimary":
                    merger.TargetKind = ILMerging.ILMerge.Kind.SameAsPrimaryAssembly;
                    break;
                default:
                    throw new BuildException( "TargetKind must be either exe, dll, winexe or sameasprimary" );
            }

            try
            {
                Log( Level.Info, "Merging assemblies to {0}.", OutputFile );
                merger.Merge();
            }
            catch( Exception e )
            {
                throw new BuildException( "Failed to merge assemblies", e );
            }
        }

		#endregion
	}
}
