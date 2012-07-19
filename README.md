NAnt-ILMerge-Extension
======================

ILMerge task for NAnt

Example usage:

&lt;loadtasks assembly="${path::combine(nant_path, 'bin\Fyhr.NAnt.ILMerge.dll')}" /&gt;

&lt;ilmerge outputfile="output.dll"
         targetkind="dll"
         logfile="ilmerge.log"
         targetplatform="v4"
         targetplatformdirectory="C:\Windows\Microsoft.NET\Framework64\v4.0.30319"&gt;
  &lt;inputassemblies>
    &lt;include name="first.dll')}" /&gt;
    &lt;include name="second.exe')}" /&gt;
  &lt;/inputassemblies&gt;

&lt;/ilmerge&gt;