   M S B A M L   `   `   `  ????M  IMusicDownloaderApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:MusicDownloaderApp.ViewsMusicDownloaderApp.Views  5      .    (MusicDownloaderApp.Views.DownloadMusicUC   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:MusicDownloaderApp.Views     5      $	??600??5      $
??1000??5	      $??15(?5
      .??5      ? 
???????  ? RowDefinitions  5   
   ?? 5      $??2* ?6   6*   ?? 5      $??* ?6   6)   ?? 5      $??8* ?6   6*   .+?5   
   ? $??0??6   .+?5      ??  ?? Orientation$ 
Horizontal=?6   .+?5      ?? $??\Assets\youtubeIcon.png??6   $??40,0,0,0q?5       J? InputBindings 5      d? 5       d? Gesture$ 	LeftClicka?6'    ?? Command 6;   ?? *YouTubeCommand+6`   5      ?? 5      $??30(?6   $??	150,0,0,0q?5      $??SemiBold"?5      $??Cascadia Code SemiBold)?5       $??Center=?5!      $??Center=?5"      $!??Download Music From YouTube??5#      5$      ?? 5&      $??\Assets\selectFolder.png??6   $??40??5'      $??40??5(       ? Cursor$
 Hand??5)       ? ToolTip$ Select Save folder path??5*      $??	140,0,0,0q?5+       5,      d? 5-      $ 	LeftClicka?6'    6;   ?? *SelectSaveFolderCommand+6i   5/      50      51      ?? 53   
   $??Bottom=?54      ? 56   
   $??1??6   .+?57      ?? $ 
Horizontal=?6   .+?58      ?? $??35??6   $	??790??59      $??2q?5:      ??5;      ?? *DownloadURL+ ?? Mode$ TwoWay=? ?? UpdateSourceTrigger$ PropertyChanged=?.??6g    5<      ?? 5=      	 ?? Gesture$	 Enter??6%    65   ?? *DownloadCommand+6[   .??5>   -   5?      ?? 5@      $??35??6   $??Download??5A         ButtonStyle#??[  5B      ???#47BDFF??5D      ?? *DownloadCommand+5E      5F      ?? 5G      $??Bottom=?6   $??0,10,0,0q?5H      5I      5J      ? 5M   
   $??2??6   .+?5N      ?? $??1??6   $??Disabled=?5O      $??Hidden=?5P      .??5Q      ??   LvItemStyle#??[ 5R      ??5Q      ?? *FileDetails+??5S      ?? 5T      
 ?? 
TargetType"
 ??6*   .R?5U      ?? #??z??6%   #??z??5V   %   ._?5W   "   ?? 5X      5Y      ??5\      ?? 5]      .R?5^      ? $
??Auto??6(   $
??1100??5_   (   $??0,0,0,0q?5`   (   5a      5b      5d      5e      5f      5g      5h      