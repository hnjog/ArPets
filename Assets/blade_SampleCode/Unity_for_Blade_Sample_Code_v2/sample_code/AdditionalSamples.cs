// Vuzix Blade Sample Code Snippits. 

/*
    // Disable the sleep timeout during gameplay. 
    // You can re-enable the timeout when menu screens are displayed as necessary.   

    ...
    void Start () {
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    ...
 */


/*
    // Use the Gyroscope to free-rotate a camera in unity. 
    ...
    void Start () {
		// Enable the gyroscope. 
		if( SystemInfo.supportsGyroscope ){
			Input.gyro.enabled = true; 
		}
	}
    ...
    void Update () {
		
		// First - Grab the Gyro's orientation. 
		Quaternion tAttitude = Input.gyro.attitude;
		// The Vuzix blade uses a 'left-hand' orientation, we need to transform it to 'right-hand'
		Quaternion tGyro = new Quaternion( tAttitude.x, tAttitude.y, -tAttitude.z, -tAttitude.w) ;
		
		// the gyro attitude is tilted towards the floor and upside-down reletive to what we want in unity.  
		// First Rotate the orientation up 90deg on the X Axis, then 180Deg on the Z to flip it right-side up. 
		Quaternion tRotation = Quaternion.Euler( -90f, 0, 0)  * tGyro;
		tRotation = Quaternion.Euler( 0,0,180f) * tRotation;

		// You can now apply this rotation to any unity camera!
		mainCamera.transform.localRotation = tRotation;
    }
    ...
*/
