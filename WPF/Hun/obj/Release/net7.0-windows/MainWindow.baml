   M S B A M L   `   `   `  ����   RPG_nagymarci_WPF    RPGProjekt.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$  RPG Játék��$	��500��$	��700�� � 
ResizeMode$ NoResize=�$��Rockwell)�����  �� 
StartPoint$	 0,0&� �� EndPoint$	 1,1&�.���  � Color$ #00837E�� � Offset$ 0Q�� $ #1E1E2E��$ 1Q�.���  � RowDefinitions �� $
��484* ��� $��* �.+��� -   $��lstKarakterek��$��2q�$
��Left=�$	��260��$��10,42,0,285q� �� DisplayMemberPath$ KarakterNev�����	#FF271F04����  �� Color$ White��	 �� Opacity$		 0.3Q��� -   $��KarakterAdatok��$
��Left=�$	��223��$��
10,246,0,0q�.+��� -   $��lblSzint��$��20(�$��Szint: ��$	��235���� -   $��lblElet��$��20(�$��Élet: ��$	��235���� -   $��lblTapasztalat��$��20(�$��Tapasztalat: ��$	��235���� -   $��lblFelszereles��$��20(�$��Felszerelés: ��$	��235���� -   $��lblPenz��$��20(�$��Pénz: ��$	��235���� -   $��
lblEnergia��$��20(�$��	Energia: ��$	��235��$��37���� $
��Left=�$	��286��$��
379,14,0,7q�.+��� -	   $��20(�$��Karakter Létrehozása��$��15q�$	��256��$��26������ $ 	#FFDDDDDD��$		 0.5Q��� -
   $��20(�$��	Támadás��$��15q�$	��256��$��27������ $ 	#FFDDDDDD��$		 0.4Q��� -   $��20(�$��Gyógyítás��$��15q�$	��256��$��26������ $ 	#FFDDDDDD��$		 0.4Q��� -   
 ��Name$
 szintfel��$��20(�$��Szintlépés (0/5)��$��15q�${�False��$	��256��$��26������ $ 	#FFC1BDBD��$		 0.4Q��� -   $��20(�$��Kalandozás��$��15q�$	��256��$��27������ $ 	#FFDDDDDD��$		 0.4Q��� -   $��20(�$��Mentés��$��15q�$	��256��$��27������ $ 	#FFDDDDDD��$		 0.4Q��� -   $��20(�$��Fejvadászat��$��15q�$	��256��$��28������ $ 	#FFDDDDDD��$		 0.4Q��� -   $��20(����#FF5B61$��	Kilépés��$��15q�$	��256��$��28���� $��23(�$��Aktív karakterek:��$
��Left=�$��0,5,0,0q�$	��Top=��� $��23(�$��Karakter infó:��$
��Left=�$��
10,204,0,0q�$	��Top=��� -   $��20(�$
��PIAC��$
��Left=�$��302,345,0,0q�$	��Top=�$��60��$��72������ $ 	#FFDDDDDD��$		 0.4Q��� -    ��Name$ CharacterSprite��$	��100��$	��100��$��170,265,430,118q�$y�	Collapsed=����� -   SpriteBrush�� �� ImageSource$ /karakterkep.png�� j� Stretch$ UniformToFill=�