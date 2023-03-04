import React from "react";

export default function FilterFolderCheckBox({label, currentState, changeState}) {

    let labelStyle = currentState ? {color:"red"} : {}

    return <>
        <label>
            {label && <span style={labelStyle}>{label}</span>}
            <input type="checkbox" style={{opacity: 0, width:0}} onChange={(e) => {e.stopPropagation(); changeState(!currentState);}}/>
        </label>
    </>;
}