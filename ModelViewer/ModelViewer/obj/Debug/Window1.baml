   M S B A M L   `   `   `  ����[  WHelixToolkit.Wpf, Version=2014.2.37.1, Culture=neutral, PublicKeyToken=52aa3500039caf0dM8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.WpfHelixToolkit.Wpf  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      F BModelViewer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ModelViewerModelViewer 5         ModelViewer.Window1   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jhelix8clr-namespace:HelixToolkit.Wpf;assembly=HelixToolkit.Wpf     5      ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     5      1modelViewerclr-namespace:ModelViewer     5      $	��480��6V   $��1103.986��6c   
���   �  � Title  63   �� *ApplicationTitle+ � 	Resources 5       �� DataType  ModelViewer.MainWindow �� ItemsSource �� Orientation �� Padding��%�  (��       5   #   *, +)�� 6
   "  6#    6Q   �� *
Children+.R�5	      �� $ Vertical=�6   .+�5
      �� $ 
Horizontal=�6   .+�5      A� $��8��6   $��8��6(   $��4q�63   
���   ���6>   �� *Brush+6U   �� 5      $
��Bold"�6:   ��6    �� *
TypeName+6:   �� 5      $ 8 0 0 0q�66   ��6    �� *Name+66   5      5      5      5      .��5      ]� -   5         ]�Name$ PanelAll��5      ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    �� 
StartPoint$ 0.5,0&�63   .��5      � 	 � Color$	 Black��6   
 � Offset$
 0Q�6-   � 5      $	 White��6   $
 1Q�6-   5      .+�5   
   o� $	��Top=�6   .@�5      m� $��_File��6   .@�5      m� $��_Open...��6    m� Command 6-   �� *FileOpenCommand+m� 5      $��
_Export...��6    6/   �� *FileExportCommand+�� 5      m� 5      $��E_xit��6    6*   �� *FileExitCommand+5       m� 5!      $��_Edit��6   .@�5"      m� $��
_Copy XAML��6    6/   �� *EditCopyXamlCommand+5#      m� 5$      $��_View��6   .@�5%      m� $��_Zoom extents��6    62   �� *ViewZoomExtentsCommand+5&      m� 5'      $��_Help��6   .@�5(      m� $��_About��6    6+   �� *HelpAboutCommand+5)      5*      ]� 5,   
   $
��Left=�6   $	��200��6*   .+�5-      ��  � Minimum$ 1Q�61    � Maximum$ 2Q�6=   $��Bottom=�6I    � Value 6   �� *	Expansion+6I   �� 5.      $��Bottom=�6   $	��200��6.   ��6;   �� *Info+R� 5/      ��50      �� *
Elements+51      53      �� 55   
   -   5         ��Name$ ViewPort3DStackPanel��55      $ 
Horizontal=�63   $
��Auto=�6L   
���r���$��Right=�6�   $	��864��6�   .+�57      &    HelixToolkit.Wpf.HelixViewport3D  -   5          Name$ view1��57   $      CameraRotationMode$ 	Trackball=�63      ModelUpDirection$ 0,1,05�6R   
���   �$��2q�6   $��Stretch=�6�   $��Stretch=�6�   $	��432��6�   )   "CoordinateSystemHorizontalPosition$ Stretch=�6  '    CoordinateSystemVerticalPosition$ Stretch=�64     Children. 5>      g� -   5        	root1�� g� Content 5>   /   �� *CurrentModel+.2�5?      (   "HelixToolkit.Wpf.GridLinesVisual3D  $   HelixToolkit.Wpf.DefaultLights  5@      5C      5D        5F      -   5        $ view2��5F   $   $ 	Trackball=�63   $ 0,1,05�6R   
���   �$��2q�6   $��Stretch=�6�   $ Stretch=�6�   $ Stretch=�6   J� RenderTransformOrigin$ 0.5,0.5&�6.  $��Stretch=�6N  $	��432��6q  z�5G      V� 5H      .~�5I      �� �� 5J       �� AngleY$ -0.021Q�6(   �� 5K      T� 5L       T� Y$ -0.08Q�6-   5M      . 5O      g� -   5        	root2�� 5O   0   �� *CurrentModel+.2�5Q          5R      5Y      5Z      5[      5\      5]      