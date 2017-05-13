# UnityChanScreenShot
Create an image of UnityChan (ToonShader Ver2).

# Base Project
It is based on UnityChan ToonShader Ver 2.  
unity-chan License is attached to the "UnityChanLicense" folder of the project.  

[UnityChanToonShaderVer2](http://unity-chan.com/download/releaseNote.php?id=UTS2_0)  
© Unity Technologies Japan/UCL  

[unity-chan License](http://unity-chan.com/contents/license_en/)  
You can switch between English and Japanese display.  

# Details
Create an alpha image of only unity-chan.  
The posture of limbs can be changed by IK.  
Lighting has been changed to the front of the camera.  

Please edit the code if you want to change the size of the image.  
[ScreenShot.cs](https://github.com/manakamic/UnityChanScreenShot/blob/master/Assets/ScreenShot.cs#L4-L5)

*Sample image(Create with this tool)*  
![Sample image](/sample.png)

# Operation
*Play ScreenShot scene with UnityEditor.*  

*W Key*  
Next animation  

*Q Key*  
Back animation  

*P key*  
Pause animation(Repeat pause and play)  

*O key*  
Create images with the current camera display  
It is created with the date file name in the project root  

*L Hand, R Hand, L Foot, R Foot Button*  
Activate IK  
You should do it during a pause  

*Pos(Rot) Button*  
Switch IK's operation to position or rotation  

*Up, Down Key*  
Move IK on Y axis of camera coordinates  

*Left, Right Key*  
Move IK on X axis of camera coordinates  

*N, M Key*  
Move IK on Z axis of camera coordinates  

*Now IK Clear Button*  
Reset the active IK now  
Return to the posture of animation  




