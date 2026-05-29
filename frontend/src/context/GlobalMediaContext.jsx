import { createContext, useContext, useRef, useState } from "react";

const GlobalMediaContext = createContext(null);

export function GlobalMediaProvider({ children }) {
  const videoRef = useRef(null);
  const [isPlaying, setIsPlaying] = useState(false);
  const [hasStarted, setHasStarted] = useState(false);

  const playMusic = async () => {
    if (!videoRef.current) return;

    try {
      await videoRef.current.play();
      setIsPlaying(true);
      setHasStarted(true);
    } catch (error) {
      console.error("Music could not start:", error);
    }
  };

  const pauseMusic = () => {
    if (!videoRef.current) return;

    videoRef.current.pause();
    setIsPlaying(false);
  };

  const toggleMusic = () => {
    if (isPlaying) {
      pauseMusic();
    } else {
      playMusic();
    }
  };

  return (
    <GlobalMediaContext.Provider
      value={{
        playMusic,
        pauseMusic,
        toggleMusic,
        isPlaying,
        hasStarted,
      }}
    >
      {children}

      {/* Hidden global video/audio that continues between pages */}
      <video
        ref={videoRef}
        src="/images/leb.mp4"
        loop
        playsInline
        style={{ display: "none" }}
        onPlay={() => setIsPlaying(true)}
        onPause={() => setIsPlaying(false)}
      />

      {/* Small floating controller after user starts music */}
      {hasStarted && (
        <div className="global-music-player">
          <div>
            <strong>Lebanon Journey 🎵</strong>
            <p>{isPlaying ? "Playing" : "Paused"}</p>
          </div>

          <button onClick={toggleMusic}>
            {isPlaying ? "Pause" : "Play"}
          </button>
        </div>
      )}
    </GlobalMediaContext.Provider>
  );
}

export function useGlobalMedia() {
  return useContext(GlobalMediaContext);
}