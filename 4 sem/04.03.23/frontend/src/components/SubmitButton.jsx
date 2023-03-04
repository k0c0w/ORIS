import Button from "antd"

const SubmitButton = ({label, onClick}) => {
    return <Button type="submit" onClick = {()=> onClick()}>{label}</Button>
}

export default SubmitButton;
