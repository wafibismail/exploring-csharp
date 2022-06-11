using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Having this interface does not seem necessary considering Dray
//   is the only class that implements it
// The author says it's to demonstrate classes implementing
//   multiple interfaces.

public interface IKeyMaster {
	int keyCount { get; set; }
	int GetFacing();
}
