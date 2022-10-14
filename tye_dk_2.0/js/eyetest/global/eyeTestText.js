var eyeTestProgramID, eyeTestUpdateToken;

/// Log a text EyeTest start
function eyeTestStartText(ProgramEyeTestID) {
	setTimeout(function () {
		eyeTestProgramID = ProgramEyeTestID;
		wsTye.EyeTestStart(ProgramEyeTestID,
			function (data) {
				// on success set interval to update end time
				// much consideration has gone into choosing this method :-/
				eyeTestUpdateToken = data;
				setInterval(function () {
					wsTye.EyeTestEnd(eyeTestProgramID, eyeTestUpdateToken, '', '', 0);
				}, 20000); // 20 second interval
			},
			function () { /* todo: error - attempt logging of error */ }
		);
	}, 5000); // wait a bit to prevent very short logging
}