NAnt-ILMerge-Extension
======================

ILMerge task for NAnt

Example usage:

<loadtasks assembly="${path::combine(nant_path, 'bin\Fyhr.NAnt.ILMerge.dll')}" />

<ilmerge outputfile="output.dll"
         targetkind="dll"
         logfile="ilmerge.log"
         targetplatform="v4"
         targetplatformdirectory="C:\Windows\Microsoft.NET\Framework64\v4.0.30319">
  <inputassemblies>
    <include name="first.dll')}" />
    <include name="second.exe')}" />
  </inputassemblies>

</ilmerge>