//Title: Protocol Manager.CS
//Author: Tyler T. Whittier
//Date: 12/16/2019
//Comments:  This script sets all the parameters for one trial of the Bayesian Balance study.  it requires selection of the public game objects: cursor, Com, target box, start box, and one of the hip objects. 
//It also requires the selction of the meshrenderer from the cursor object, and the input of the noise level, the angle of the target to appear, the percent of hip height that will determine the target radius, 
//the amount of offset for objects in front of participants, and the degree of shift for X and Z dimensions.



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ProtocolManager : MonoBehaviour
{
    #region DeclareVariables
	//Public Drag-n-drops
    public GameObject Cursor; //Object for the cursor that participants see and percieve as their CoM
	public GameObject Cursor_FB; //Object that is shown to give feedback
	public GameObject CoM;//Object streaming from motion capture of their CoM
	public GameObject Target;//The target box
	public GameObject StartBox;//The startBox
	public GameObject Hip;//One of the two hip objects.  This object is used to determine how far the target will be from the startbox
	public MeshRenderer rend; //The meshrenderer of the cursor
	public MeshRenderer FB_rend;//Renderer for the feedback object

	//Public Fill-ins
	public float NoiseSigma; // The Variability of the noise added to the object location
	public float PercentHipHeight; //The percent of the hip height object that dictates how far the target will be
	public float OFFSET;//This variable shifts all objects forward so as to be seen by the participant rather than underneath
	public float OFFSET_up;//This variable shifts all objects up so as to be seen by the participant rather than underneat
	public int N_trials;
	public float MU;
	public float sigma;
	public float[] freq = new float[4];
	
	//Public Arrays-DONT TOUCH!!!
	public float[] NoiseList;
	public float[] AngleList;
	public float[] ShiftList;
	public float[] ZeroDist;
	public float[] SmallDist;
	public float[] LargeDist;
	public float[] AllDist;
	public float[] CurrentDist;

	//Public Safety Checks-DONT TOUCH!!
	public float Noise;
	public float Angle;
	public float Shift;
	public int NoiseCount0 = 0;
	public int NoiseCount1 = 0;
	public int NoiseCount2 = 0;
	public int NoiseCountInf = 0;
	public int Trialcount = 0;
	public bool Feedback = false;
	




	//These variables use the public variables to handle the logistics of performing a trial
	private float ShiftX;//This is the X component of the shift added to the cursor location to build the prior
	private float ShiftZ;//This is the Z component of the shift added to the cursor location to build the prior
	private Vector3 Tru_shift;
	private Vector3 Push_Forward;
	private Vector3 Push_up;
	private float P;
	private float XlengtH;
	private float ZlengtH;
    private float TX;
	private float TZ;
	private float SX;
	private float SZ;
	private Vector3 Noise_vec;
	private Vector3 Cursorloc;
	private float SB_RangeX;
	private float SB_MinX;
	private float SB_MaxX;
	private Vector3 StartSpace;
	private float StartDist;
	private float Shift_Dist;
	private float Rad_Dist;
	private float Rad_Min;
	private float Rad_Max;
	private Vector3 Cursorloc_w_shift;
	private float N_Level;
	private float Hypotenuse;
	private float Theta;
	//private bool Feedback = false;

	#endregion



	#region Array Builder
	void Awake()
	{
		//Declare The hypotenuse length of the step length for every trial
		

		#region Angle List
		//Create an n length array of angles for the target to appear in front of participant.  There are 5 possibilities (90, 135, 180, 225, 270)
		AngleList = new float[N_trials];
		int temp = N_trials / 5;
		float[] East = new float[temp];
		float[] NEast = new float[temp];
		float[] North = new float[temp];
		float[] NWest = new float[temp];
		float[] West = new float[temp];

		for (int i = 0; i < temp; i++)
		{
			East[i] = 90f;
			NEast[i] = 135f;
			North[i] = 180f;
			NWest[i] = 225f;
			West[i] = 270f;
		}

		East.CopyTo(AngleList, 0);
		NEast.CopyTo(AngleList, temp);
		North.CopyTo(AngleList, temp * 2);
		NWest.CopyTo(AngleList, temp * 3);
		West.CopyTo(AngleList, temp * 4);
		for (int i = 0; i < AngleList.Length; i++)
		{
			int rnd = Random.Range(0, AngleList.Length);
			float tempGO = AngleList[rnd];
			AngleList[rnd] = AngleList[i];
			AngleList[i] = tempGO;
		}
		#endregion

		#region Noise List
		//Create an array of length n with the noise levels for every trial.  Taking in the relative frequencies of each noise level
		NoiseList = new float[N_trials];
		float temp2 = 0;

		for (int i = 0; i < freq.Length; i++)
		{
			temp2 = temp2 + freq[i];//sum up the values in the "freq" array
		}
		float Interval = 1f / temp2;// divide by 1 to get the interval that we'll use for selecting noise level
		float zeroNoiseRng = Interval * freq[0]; // multiply the interval by the 1st value in freq so we'll have the range for the 0 noise level condition
		float range = 1 - zeroNoiseRng;//determine the rest of the range that we will devide by the other three conditions to determine each of their ranges
		range = range / 3f;

		for (int i = 0; i < N_trials; i++)//sort the noise level values with the frequency level determined form the public float array
		{
			float rnd = Random.value;
			if (rnd <= zeroNoiseRng)
			{
				NoiseList[i] = 0;
				NoiseCount0 = NoiseCount0 + 1;
			
			}
			if (rnd <= (zeroNoiseRng + range) && (rnd > (zeroNoiseRng)))
			{
				NoiseList[i] = 1;
				NoiseCount1 = NoiseCount1 + 1;
			}
			if (rnd <= (zeroNoiseRng + (range * 2f)) && (rnd > (zeroNoiseRng + range)))
			{
				NoiseList[i] = 2;
				NoiseCount2 = NoiseCount2 + 1;
			}
			if (rnd <= (zeroNoiseRng + (range * 3f)) && (rnd > (zeroNoiseRng + (range * 2f))))
			{
				NoiseList[i] = 3;
				NoiseCountInf = NoiseCountInf + 1;
			}

		}
		#endregion

		#region Noise Distributions
		//Calculate a noise distribution to be added to the cursor position to add variability to the likelihood
		// Zero Noise
		ZeroDist = new float[2000];
		for (int i = 0; i < 1999; i++)//generate a random number from a normal distribution for every trial
		{
			ZeroDist[i] = 0;
		}

		//Small Noise distribution
		SmallDist = new float[2000];
		for (int i = 0; i < 1999; i++)//generate a random number from a normal distribution for every trial
		{
			float u1 = 1.0f - Random.Range(0f, 1f);
			float u2 = 1.0f - Random.Range(0f, 1f);
			float MyRand = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
			SmallDist[i] = MyRand * NoiseSigma;//Multiply by sigma and add the mean to put it in the scope of the shift that we want
		}

		LargeDist = new float[2000];
		for (int i = 0; i < 1999; i++)//generate a random number from a normal distribution for every trial
		{
			float u1 = 1.0f - Random.Range(0f, 1f);
			float u2 = 1.0f - Random.Range(0f, 1f);
			float MyRand = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
			LargeDist[i] = MyRand * (NoiseSigma*2);//Multiply by sigma and add the mean to put it in the scope of the shift that we want
		}
        #endregion

        #region Shift List;
        //Create an array of length N_trials of random values chosen from a normal distribution with mean mu and std sigma
        ShiftList = new float[N_trials];
		for (int i = 0; i < N_trials; i++)//generate a random number from a normal distribution for every trial
		{
			float u1 = 1.0f - Random.Range(0f, 1f);
			float u2 = 1.0f - Random.Range(0f, 1f);
			float MyRand = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
			ShiftList[i] = MyRand * sigma + MU;//Multiply by sigma and add the mean to put it in the scope of the shift that we want
		}

		// All Noise
		AllDist = new float[2000];
		for (int i = 0; i < 1999; i++)//generate a random number from a normal distribution for every trial
		{
			AllDist[i] = 1000;
		}
		#endregion
	}

    #endregion


	// Start is called before the first frame update
	void OnEnable()
	{
		FB_rend.enabled = false;
		Noise = NoiseList[Trialcount];
		Angle = AngleList[Trialcount];
		Shift = ShiftList[Trialcount];
		P = PercentHipHeight / 100;
		Hypotenuse = (Hip.transform.position.y * P);

		#region StartLocations
		//Place the Start and target locations based off of Hip object, CoM object and the offset



		//Vector3 offset = Quaternion.AngleAxis(Angle, CoM.transform.up) * CoM.transform.forward * (Hip.transform.position.y * P);
		// Vector3 Global = transform.TransformPoint(Vector3.right * 2);
		//Target.transform.position = CoM.transform.position + offset;
		//StartBox.transform.position = CoM.transform.position;
		//      //Target location X and Z coordinates
		//////TX = offset.x;
		//////TZ = offset.z;
		TX = (Hypotenuse * Mathf.Cos(Angle * Mathf.Deg2Rad)) + OFFSET + CoM.transform.position.x;
	      TZ = (Hypotenuse * Mathf.Sin(Angle * Mathf.Deg2Rad))+ CoM.transform.position.z;
		//      //Start box location (X and Z coordinates) and radius of where the cursor leaves the startbox
		SX = CoM.transform.position.x + OFFSET;
        SZ = CoM.transform.position.z;
        float Tempx2 = StartBox.transform.localScale.x;
        SB_RangeX = Tempx2 / 2;


		#endregion

		#region ShiftCoM
		//Create vectors for where the CoM will appear throughout a trial

		ShiftX = (Shift * Mathf.Cos((Angle + 180) * Mathf.Deg2Rad));
		ShiftZ = (Shift * Mathf.Sin((Angle + 180) * Mathf.Deg2Rad));
		
		Tru_shift = new Vector3(ShiftX, 0, ShiftZ);
		Push_Forward = new Vector3(OFFSET, 0, 0);
		Push_up = new Vector3(0, OFFSET_up, 0);
		#endregion

		#region Visibility
		rend = GameObject.Find("Cursor").GetComponent<MeshRenderer>();
		rend.enabled = true;
        #endregion

        #region Noise Level
		//Set noise level depending on the inputed noise level and the scale determined from the noise array
        if (Noise == 0)
		{
			CurrentDist = ZeroDist;
		}
		else if (Noise == 1)
        {
			CurrentDist = SmallDist;
		}
		else if (Noise == 2)
        {
			CurrentDist = LargeDist;
		}
		else if (Noise == 3)
		{
			CurrentDist = AllDist;
		}
        #endregion
    }

	void OnDisable()
    {
		Trialcount = Trialcount + 1;
		Feedback = false;
		FB_rend.enabled = false;
	}
 

    // Update is called once per frame
    void Update()
    {
		#region ObjectLocations
		//update locations of all objects 
		Target.transform.position = new Vector3(TX, CoM.transform.position.y, TZ) + Push_up;
		StartBox.transform.position = new Vector3(SX, CoM.transform.position.y, SZ) + Push_up;
		Cursorloc = CoM.transform.position + Push_Forward + Push_up;
		Cursorloc_w_shift = Cursorloc+ Tru_shift;
		//Calculate distances and radii that assist with when visual feedback is given
		Rad_Dist = Mathf.Abs(Vector3.Distance(StartBox.transform.position, Target.transform.position));
		Vector3 Rad_Vec = Target.transform.position - StartBox.transform.position;
		
		//Shift_Dist = Mathf.Abs(Vector3.Distance(Cursor.transform.position, StartBox.transform.position));
		
		Rad_Min = Rad_Dist * .20f;
		Rad_Max = Rad_Dist * .70f;
		//Create a noise vector that is added to the position of the cursor 
		Noise_vec = new Vector3(CurrentDist[Random.Range(0, 1999)], 0, CurrentDist[Random.Range(0, 1999)]);
		Cursor.transform.position = Cursorloc;
		Vector3 Curs_Vec = Cursor.transform.position - StartBox.transform.position;
		//Theta = Vector3.Angle(Cursor.transform.position - StartBox.transform.position, Target.transform.position - StartBox.transform.position);
		//Debug.Log("Theta is "  + Theta + " Rad_Min is " + Rad_Min + " Rad_Max is " + Rad_Max + "Shift_Dist is " + Shift_Dist + "Rad_Dist is " + Rad_Dist + StartBox.transform.position + Target.transform.position + Cursor.transform.position);
		
		
		//StartDist is the distance from the cursor to the startbox.  this is used when determining when participants can see the cursor
		StartDist = Mathf.Abs(Vector3.Distance(Cursor.transform.position, StartBox.transform.position));
		#endregion

		#region Visibility Shift and Noise
		//The startbox is set to be located where the CoM cursor is located before a trial begins.Once the cursor leaves the startbox,
		//it is occluded from vision and the shift is added to the cursor location

		if (StartDist > SB_RangeX)
		{

			rend.enabled = false;
			Cursor.transform.position = Cursorloc + Tru_shift + Noise_vec;
			Theta = Vector3.Angle(Cursor.transform.position - StartBox.transform.position, Target.transform.position - StartBox.transform.position);
			Shift_Dist = Mathf.Abs(Vector3.Distance(Cursor.transform.position, StartBox.transform.position));
			//This if statement allows visual feedback while the cursor is in the middle % 10 of the movement between start and target
			if ((Shift_Dist > Rad_Min) && (Shift_Dist < Rad_Max) && Theta < 45f)
			{
				Cursor.transform.position = Cursorloc + Tru_shift + Noise_vec;

				rend.enabled = true;
			}

		}
		#endregion



		//This statement exports the study data at the moment that the button is clicked to a csv with the title report
		if (SteamVR_Actions._default.Select.GetStateUp(SteamVR_Input_Sources.Any))
        {
		CSVManager.AppendToReport(GetReportLine());
		Debug.Log("Report updated successfully!");
		}
		if (SteamVR_Actions._default.Select.GetStateUp(SteamVR_Input_Sources.Any) && Noise == 0)
		{
			//Transform CursorTransform = Cursor_FB.transform;
			Cursor_FB.transform.position = Cursorloc_w_shift;
			Feedback = true;
		}

		if (Feedback == true)
		{
			FB_rend.enabled = true;
		}
		if (Feedback == false)
        {
			FB_rend.enabled = false;
		}
	}
	#region ExportData
	string[] GetReportLine() {
	string[] returnable = new string[14];
	returnable[0] = Cursorloc_w_shift.x.ToString();//Location of the cursor with the shift
	returnable[1] = Cursorloc_w_shift.z.ToString();
	returnable[2] = Cursorloc.x.ToString();//location of the cursor w/out the shift
	returnable[3] = Cursorloc.z.ToString();
	returnable[4] = CoM.transform.position.x.ToString();//location of the CoM without offset and shift added
	returnable[5] = CoM.transform.position.z.ToString();
	returnable[6] = Target.transform.position.x.ToString();//location of the target
	returnable[7] = Target.transform.position.z.ToString();
	returnable[8] = StartBox.transform.position.x.ToString();//location of the start box
	returnable[9] = StartBox.transform.position.z.ToString();
	returnable[10] = Shift.ToString();//amount of shift
	returnable[11] = ShiftZ.ToString();
	returnable[12] = Angle.ToString();//amount of shift
	returnable[13] = Noise.ToString();
		return returnable;
   }
	#endregion
}
