# TestTask

INTO: Thanks for visiting!

Tools used:
VS2019
Frameworks:
.Net 4.7.2

Packages dependencies:
  package id="FSharp.Core" version="4.5.2"  
  package id="HtmlAgilityPack" version="1.11.7"  
  package id="ScrapySharp" version="3.0.0"  

To download:

zip archive
https://github.com/Rez1st/TestTask/archive/master.zip

Or

use git tools
git clone https://github.com/Rez1st/TestTask.git


To Run 

1: Open solution with visual studio  
2: Build it  
  
[Tip: There are 2 modes in this app: manual and automated]  
3: Before running in any mode make sure you set it in app.config file if it set to false - it means app in manual mode
**key="ReadFilesFromFolder" value="false"**  
 3.1: Manual mode means you run it from the command line with parameter e.g. **App.exe C:\Temp\file1.html**  
 3.2: Auto mode **key="ReadFilesFromFolder" value="true"** means - program will pick all .html files from folder
     theres a variable **key="PathToFiles"** in app.config file - were you can set path to folder. By default it points to the
     _FilesToProcess_ folder that located in the solution folder. I've already placed test files in that folder.  
 _NOTE! If you will run software in auto mode from command line - it will ignore params that will be passed to._  
4: After you will run app in any mode it will give you output, or validation message/s  

To configure:  
There are several key config features to configure search:  
**MakeButtonFinderOptions**  
      key="text" value="OK"   
      key="href" value="#ok"   
      key="title" value="Make-Button"   
      key="id" value="make-everything-ok-button"   
      key="class" value="btn-success"   
      _**Description**_: key is an attribute we can add\remove attributes depending on how we want to search for tags
                         value in this case is simple string that will use Contains(), so it will take "text" attribute and will do                             .Contains(value)   
                        More values will hit the case more chances for Tag to be selected   
**MakeButtonTagContainer**   
       key="tag" value="a"   
      _**Description**_: to simplify search - we will apply previously described finder options only on items with tag definded here.
                         In test task I made assumption it will be in <a /> tag   
**TagFinderCustomOptions**  
      <BootstrapCustomOption>  
        key="attribute" value="class"  
        key="pattern" value="btn-"  
        key="validator" value="btn-success"  
      </BootstrapCustomOption>  
     _**Description**_: This is basically custom rules that will check does found item corresponds our custom needs
                        in this case it indicates standart bootstrap class "btn-" and make sure it will be used only with 
                        "success" modifier, meaning only green.
                        We can add as many custom rules as we want to, by specifying attribute, pattern and validator  


**Conclusion** : For any uncovered question on decision making please contact using github/email/skype
