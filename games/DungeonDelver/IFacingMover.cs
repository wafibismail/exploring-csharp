using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFacingMover { // The public declaration of the IFacingMover interface
	int GetFacing();
	bool moving { get; }
	float GetSpeed();
	float gridMult { get; }
	Vector2 roomPos { get; set; }
	Vector2 roomNum { get; set; }
	Vector2 GetRoomPosOnGrid( float mult = -1);
	// ^ if this default value for mult disagrees with any script that implements this
	//     this default value will take precedence and override the default value in the
	//     implementing classes
}
