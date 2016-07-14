set setup_dir=C:\flat4me\Services\Flat4Me.ImageService\
set output_dir=..\..\output\Release\

call 0_uninstall.bat %setup_dir%
call 1_undeploy.bat %setup_dir%
call 2_deploy.bat %setup_dir% %output_dir% 
call 3_install.bat %setup_dir%

 