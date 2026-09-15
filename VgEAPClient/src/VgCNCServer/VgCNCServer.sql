CREATE DATABASE `vg_cnc_db`;

use `vg_cnc_db`;

CREATE TABLE sysDrillInformation(
	 sInnerID           nvarchar( 500)  NOT NULL PRIMARY KEY,
	 sEquipmentID       nvarchar( 500)  NULL,
	 sDuty              nvarchar( 500)  NULL,
	 sWorkMode          nvarchar( 500)  NULL,
	 sPersent           nvarchar( 500)  NULL,
	 sDrilled           nvarchar( 500)  NULL,
	 sNeeded            nvarchar( 500)  NULL,
	 sActProgram        nvarchar( 500)  NULL,
	 sDiaFileName       nvarchar( 500)  NULL,
	 sRegistrationDate  nvarchar( 500)  NULL,
	 sStatus            nvarchar( 500)  NULL,
	 dtDateTime         datetime    NULL
) ;


CREATE TABLE sysLoginLog (
	     sInnerID               nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     sEquipmentID           nvarchar( 500)  NULL,
	     sLoginTime             nvarchar( 500)  NULL,
	     sAuthorizationCode     nvarchar( 500)  NULL,
	     sMemo                  nvarchar( 500)  NULL
)   ;


CREATE TABLE     sysStaff     (
	     sInnerID           nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     sLoginName         nvarchar( 500)  NULL,
	     sEName             nvarchar( 500)  NULL,
	     sCName             nvarchar( 500)  NULL,
	     sPWD               nvarchar( 500)  NULL,
	     sRole              nvarchar( 500)  NULL,
	     sAddTime           nvarchar( 500)  NULL,
	     sAddUser           nvarchar( 500)  NULL,
	     sMemo              nvarchar( 500)  NULL,
	     iStatus            int      NULL
)   ;


CREATE TABLE     usrAlarm     (
	     sInnerID           nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate             date      NULL,
	     sStartTime         nvarchar( 500)  NULL,
	     sEndTime           nvarchar( 500)  NULL,
	     sEquipmentID       nvarchar( 500)  NULL,
	     sAlarmID           nvarchar( 500)  NULL,
	     sAlarmDesc         nvarchar( 500)  NULL,
	     sActProgram        nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrAnalysisDuty     (
	     sInnerID                       nvarchar( 500)  NULL,
	     sEquipmentID                   nvarchar( 500)  NULL,
	     sZ                             nvarchar( 500)  NULL,
	     dtDate                         date      NULL,
	     sTime                          nchar     (10) NULL,
	     sChangePanels                  nvarchar( 500)  NULL,
	     sStandardChangePanelsTime      nvarchar( 500)  NULL,
	     sChangeDrills                  nvarchar( 500)  NULL,
	     sStandardChangeDrillsTime      nvarchar( 500)  NULL,
	     sExceptionHandleCounts         nvarchar( 500)  NULL,
	     sExceptionHandleTime           nvarchar( 500)  NULL,
	     sMaintainTime                  nvarchar( 500)  NULL,
	     sTheoryDuty                    nvarchar( 500)  NULL,
	     sActualDuty                    nvarchar( 500)  NULL,
	     sDifference                    nvarchar( 500)  NULL,
	     sShift                         nvarchar( 500)  NULL
)   ;



CREATE TABLE     usrCOMM     (
	     sInnerID               nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate                 date      NULL,
	     sTime                  nvarchar( 500)  NULL,
	     sEquipmentID           nvarchar( 500)  NULL,
	     sCOMM                  nvarchar( 500)  NULL,
	     sActProgram            nvarchar( 500)  NULL,
	     sEventCode             nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrDuty     (
	     sInnerID              nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate                date      NULL,
	     sEquipmentID          nvarchar( 500)  NULL,
	     sDutyShiftA           nvarchar( 500)  NULL,
	     sDutyShiftB           nvarchar( 500)  NULL,
	     sDutyShiftC           nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrEvent     (
	     sInnerID           nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate             date      NULL,
	     sTime              nvarchar( 500)  NULL,
	     sEquipmentID       nvarchar( 500)  NULL,
	     sEventID           nvarchar( 500)  NULL,
	     sEventDesc         nvarchar( 500)  NULL,
	     sActProgram        nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrM54Event     (
	     sInnerID           nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate             date      NULL,
	     sTime              nvarchar( 500)  NULL,
	     sEquipmentID       nvarchar( 500)  NULL,
	     sEventID           nvarchar( 500)  NULL,
	     sEventDesc         nvarchar( 500)  NULL,
	     sActProgram        nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrRealTimeDuty     (
	     sInnerID            nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     sZ                  nvarchar( 500)  NULL,
	     dtDate              date      NULL,
	     sTime               nchar     (10) NULL,
	     sEquipmentID        nvarchar( 500)  NULL,
	     sDuty               nvarchar( 500)  NULL,
	     sWorkTime           nvarchar( 500)  NULL,
	     sWaitTime           nvarchar( 500)  NULL,
	     sStopTime           nvarchar( 500)  NULL,
	     sTotalTime          nvarchar( 500)  NULL,
	     sHits               nvarchar( 500)  NULL,
	     sRoutPath           nvarchar( 500)  NULL,
	     sChangePanels       nvarchar( 500)  NULL,
	     sChangeDrills       nvarchar( 500)  NULL,
	     sRegistrationDate   nvarchar( 500)  NULL,
	     sShift              nvarchar( 500)  NULL
)   ;



CREATE TABLE     usrShiftDuty     (
	     sInnerID            nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     sZ                  nvarchar( 500)  NULL,
	     dtDate              date      NULL,
	     sTime               nchar     (10) NULL,
	     sEquipmentID        nvarchar( 500)  NULL,
	     sDuty               nvarchar( 500)  NULL,
	     sT                  nvarchar( 500)  NULL,
	     sWorkTime           nvarchar( 500)  NULL,
	     sWaitTime           nvarchar( 500)  NULL,
	     sStopTime           nvarchar( 500)  NULL,
	     sTotalTime          nvarchar( 500)  NULL,
	     sHits               nvarchar( 500)  NULL,
	     sShiftHits          nvarchar( 500)  NULL,
	     sTools              nvarchar( 500)  NULL,
	     sBrokens            nvarchar( 500)  NULL,
	     sChangePanels       nvarchar( 500)  NULL,
	     sChangeDrills       nvarchar( 500)  NULL,
	     sChangeDrillsTime   nvarchar( 500)  NULL,
	     sRegistrationDate   nvarchar( 500)  NULL,
	     sShift              nvarchar( 500)  NULL
)   ;



CREATE TABLE     usrToolsBroken     (
	     sInnerID           nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate             date      NULL,
	     sTime              nvarchar( 500)  NULL,
	     sEquipmentID       nvarchar( 500)  NULL,
	     sActProgram        nvarchar( 500)  NULL,
	     sBrokens           nvarchar( 500)  NULL,
	     sSpindle           nvarchar( 500)  NULL,
	     sToolID            nvarchar( 500)  NULL,
	     sToolDIA           nvarchar( 500)  NULL,
	     sHoleID            nvarchar( 500)  NULL,
	     sBrokenLife        nvarchar( 500)  NULL,
	     sX                 nvarchar( 500)  NULL,
	     sY                 nvarchar( 500)  NULL,
	     sProgramBlock      nvarchar( 500)  NULL,
	     sProgramStep       nvarchar( 500)  NULL
)   ;


CREATE TABLE     usrToolsBrokenEnd     (
	     sInnerID               nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate                 date      NULL,
	     sTime                  nvarchar( 500)  NULL,
	     sEquipmentID           nvarchar( 500)  NULL,
	     sActProgram            nvarchar( 500)  NULL,
	     sBrokens               nvarchar( 500)  NULL,
	     sSpindle               nvarchar( 500)  NULL,
	     sToolID                nvarchar( 500)  NULL,
	     sToolDIA               nvarchar( 500)  NULL,
	     sHoleID                nvarchar( 500)  NULL,
	     sX                     nvarchar( 500)  NULL,
	     sY                     nvarchar( 500)  NULL,
	     sProgramBlock          nvarchar( 500)  NULL,
	     sProgramStep           nvarchar( 500)  NULL
)   ;

CREATE TABLE     usrWorkingCondition     (
	     sInnerID               nvarchar( 500)  NOT NULL  PRIMARY KEY,
	     dtDate                 date      NULL,
	     sTime                  nvarchar( 500)  NULL,
	     sEquipmentID           nvarchar( 500)  NULL,
	     sActProgram            nvarchar( 500)  NULL,
	     sNeeded                nvarchar( 500)  NULL,
	     sStartTime             nvarchar( 500)  NULL,
	     sEndTime               nvarchar( 500)  NULL,
	     sWorkTime              nvarchar( 500)  NULL,
	     sWaitTime              nvarchar( 500)  NULL
)   ;

