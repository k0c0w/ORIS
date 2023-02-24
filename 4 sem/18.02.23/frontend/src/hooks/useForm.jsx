import { useState } from "react";

function useForm({ form, additionalData, endpointUrl }) {
  const [status, setStatus] = useState("");
  const [message, setMessage] = useState("");

  const handleSubmit = (event) => {
    if (form) {
      event.preventDefault();
      setStatus("loading");
      setMessage("");

      const finalFormEndpoint = endpointUrl || form.action;
      const data = Array.from(form.elements)
        .filter((element) => element.name)
        .reduce(
          (obj, input) => Object.assign(obj, { [input.name]: input.value }),
          {}
        );

      if (additionalData) {
        Object.assign(data, additionalData);
      }

      fetch(finalFormEndpoint, {
        method: "POST",
        headers:  { 'Content-type' : 'application/json', Accept : 'application/json'},
        body: JSON.stringify(data),
      })
        .then((response) => {
          if (response.status !== 200) {
            throw new Error(response.statusText);
          }

          return response.json();
        })
        .then(() => {
          setStatus("success");
        })
        .catch((err) => {
          setMessage(err.toString());
          setStatus("error");
        });
    }
  };

  return { handleSubmit, status, message };
}

export default useForm;