import React from "react"


export default function FilterFolderSearch({label, traitName}) {
    return <>
        <div style={{display: "inline-block", margin:"10px"}}>
            <span>{label}</span>
            <div>
                <input style={{width: "50px", marginRight: "5px"}} type="number" placeholder="from" name={`min_${traitName}`}/>
                <input style={{width: "50px",}} type="number" placeholder="to" name={`max_${traitName}`}/>
            </div>
        </div>
    </>
}