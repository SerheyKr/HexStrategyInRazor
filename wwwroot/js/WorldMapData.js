import { jsonProperty, Serializable } from "ts-serializable";
import { WorldRowData } from "./WorldRowData";

export class WorldMapData extends Serializable {
    @jsonProperty()
    public rows: WorldRowData[];
}
