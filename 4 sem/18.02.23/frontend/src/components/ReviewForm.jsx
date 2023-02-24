import React, { useRef, useState } from "react";
import { useNavigate } from "react-router-dom";
import { ReviewURL } from "./APIKEY";

export default function ReviewForm(){
    const formRef = useRef(null);
    const [status, setStatus] = useState("");
    const navigator = useNavigate();

    function handleSubmit(event) {
        if(formRef){
            const endpointUrl = ReviewURL;
            const form = formRef.current;
            event.preventDefault();
            const data = Array.from(form.elements)
                        .filter((element) => element.name)
                        .reduce((obj, input) => Object.assign(obj, {[input.name]: input.value}), {});
            data.sent = new Date().toISOString();
  
            fetch(endpointUrl, {
                method: "POST",
                headers: {
                    "Accept" : "application/json",
                    "Content-Type" : "application/json"
                },
                body: JSON.stringify(data)
            })
            .then((response) => {
                console.log(response.status);
                if (response.status === 200) {
                    let json = response.json();
                    if(json && json.error){
                        setStatus("error");
                    }
                    else { setStatus("success"); }
                }
                else{
                    setStatus("error");
                }
                return response.json();
              });
        }
    }

    if (status === "success") {
        return(navigator('/'));
      }

    return (
    <>
    <h1>Leave a review about our service!</h1>
    <form ref={formRef} method="POST" action={ReviewURL} onSubmit={handleSubmit}>
        <div>
            <h4>Your email</h4>
            <input type="email" placeholder="exareviewmple@gmail.com" required name="email"></input>
        </div>
        <div>
        <h4>Your name</h4>  
            <input type="text" placeholder="John" required name="name"></input>
        </div>
        <div>
            <h4>Type review here</h4>
            <textarea type="text" required maxLength={300} name="review"></textarea>
        </div>
        {status === "error" && <p style={{color:"red"}}>Error occured, try again</p>}
        {status !== "loading" && <button type="submit">Send</button>}
    </form>
    </>);
}