import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";
import "../styles/JourneyEditor.css";

export default function JourneyEditor() {

  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem("user"));

  const [title,setTitle] = useState("");
  const [content,setContent] = useState("");
  const [media,setMedia] = useState(null);
  const [preview,setPreview] = useState(null);

  const saveJourney = async () => {

    if(!title.trim() || !content.trim())
      return alert("Please write title and story");

    const formData = new FormData();

    formData.append("userId", user.id);
    formData.append("title", title);
    formData.append("content", content);

    if(media) formData.append("media", media);

    await api.post("/journey", formData,{
      headers:{ "Content-Type":"multipart/form-data"}
    });

    navigate("/profile/journeys");
  };

  return (

    <div className="journalEditorPage">

      <div className="journalPaper">

        <div className="editorToolbar">

          <button
            className="btnBack"
            onClick={()=>navigate("/profile/journeys")}
          >
            ← Back
          </button>

          <button
            className="btnSave"
            onClick={saveJourney}
          >
            Save Journey
          </button>

        </div>

        <input
          className="journalTitle"
          placeholder="Title of your memory..."
          value={title}
          onChange={(e)=>setTitle(e.target.value)}
        />

        <textarea
          className="journalContent"
          placeholder="Write your story here..."
          value={content}
          onChange={(e)=>setContent(e.target.value)}
        />

        <div className="mediaUpload">

          <label>Add photo or video</label>

          <input
            type="file"
            accept="image/*,video/*"
            onChange={(e)=>{
              const file = e.target.files[0];
              if(file){
                setMedia(file);
                setPreview(URL.createObjectURL(file));
              }
            }}
          />

        </div>

        {preview && (
          <div className="editorPreview">
            {media.type.startsWith("video") ?
              <video src={preview} controls/> :
              <img src={preview} alt="preview"/>
            }
          </div>
        )}

      </div>

    </div>
  );
}