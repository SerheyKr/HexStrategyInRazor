import { jsonProperty, Serializable } from "ts-serializable";
import { WorldCellData } from "./WorldCellData";


export class WorldRowData extends Serializable {
    @jsonProperty()
    public cells: WorldCellData[];
}

