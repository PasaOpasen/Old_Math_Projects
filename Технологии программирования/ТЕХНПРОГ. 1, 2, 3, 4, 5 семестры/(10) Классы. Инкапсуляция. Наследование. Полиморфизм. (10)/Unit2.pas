unit Unit2;

interface

implementation

	uses
		SysUtils,
		Dialogs;

	type Resident = class
              apartament : Integer;
              Surrname : String;
              function Info (): String;
      end;
	function Resident.Info;
		begin
			Result := 'Житель: ' + Surrname +'; номер квартиры: ' + IntToStr(apartament) + '';
		end;

	Var
		Ivanov : Resident;
	begin
		Ivanov := Resident.Create();
		Ivanov.apartament:=100;
		Ivanov.Surrname:='ПАСЬКО Д. А.';
		Showmessage(Ivanov.Info());
	End.


end.
 