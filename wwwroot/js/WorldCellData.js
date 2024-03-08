import { jsonProperty, Serializable } from "ts-serializable";


export class WorldCellData extends Serializable {
	@jsonProperty()
	public buildingsCount: int;
	@jsonProperty()
	public defenceCount: int;
	@jsonProperty()
	public positionId: string;
}