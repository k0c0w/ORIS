import React, { useState, useRef } from "react";
import FilterFolderCheckBox from "../components/FilterFolderCheckBox.jsx";
import FilterFolderSearch from "../components/FilterFolderSearch.jsx";

const searchBarStyle = {
    width: "auto",
    border: "white 1px solid",
    borderBottomRightRadius: "20px",
    borderBottomLeftRadius: "20px",
    padding: "20px",
    backgroundColor: "#ced7e7",
    marginBottom: "20px",
    color: "black",

}


export default function FilterFolder({setFilterQuery}) {
    const [lifespanEnabled, setLifespanEnabled] = useState(false);
    const [weightspanEnabled, setWeightspanEnabled] = useState(false);
    const [heightspanEnabled, setHeightspanEnabled] = useState(false);
    const [filterEnabled, setFilterEnabled] = useState(false);
    const formRef = useRef(null);

    function handleSubmit(event) {
        event.preventDefault();
        if(formRef){
            const query = Array.from(formRef.current.elements)
                        .filter((element) => element.name && element.value)
                        .map(input => `${encodeURIComponent(input.name)}=${encodeURIComponent(input.value)}`)
                        .join('&');
            setFilterQuery(query);
            setFilterEnabled(true);
        }
    }

    function disableFilter(formRef) {
        if(formRef) {
            Array.from(formRef.current.elements)
            .filter(element => element.name && element.value)
            .forEach(element => element.value = '');
            setFilterQuery('');
            setFilterEnabled(false);
        }
    }
    

    return <div style={searchBarStyle}>
        <div style={{display:"flex", alignItems:"center", flexDirection: "column"}}>
            <span style={{fontWeight: "bold"}}>Choose filter settings</span>
            <div>
            <FilterFolderCheckBox currentState = {lifespanEnabled} changeState = {setLifespanEnabled} 
            label="Life span"/>
            <FilterFolderCheckBox currentState = {weightspanEnabled} changeState = {setWeightspanEnabled} 
            label="Weight span"/>
            <FilterFolderCheckBox currentState = {heightspanEnabled} changeState = {setHeightspanEnabled} 
            label="Height span"/>
            </div>
        </div>
    
        <form ref={formRef} onSubmit={handleSubmit}>
            {lifespanEnabled && <FilterFolderSearch label="Life span" traitName="lifespan"/>}
            {weightspanEnabled && <FilterFolderSearch label="Weight span" traitName="weight"/>}
            {heightspanEnabled && <FilterFolderSearch label="Height span" traitName="height"/>}
            {
                <label>
                    <span>Breed group</span>
                    <input type="text" name="breed_group" placeholder="breed_group"/>
                </label>
            }
        <button type="submit">Filter</button>
        {filterEnabled && 
            <svg onClick={(e) => {e.preventDefault(); disableFilter(formRef);}} transform="translate(5px, 5px)" xmlns="http://www.w3.org/2000/svg" width="20px" height="20px" viewBox="0 0 24 24" fill="none">
            <path d="M16 8L8 16M8.00001 8L16 16" stroke="#000000" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"/>
            </svg>}
        </form>
    </div>;
}