import i18n from "i18next";
import { initReactI18next } from "react-i18next";

import en from "./locales/en.json";
import ar from "./locales/ar.json";
import fr from "./locales/fr.json";

// 🔥 Load saved language or default
const savedLang = localStorage.getItem("lang") || "en";

i18n
  .use(initReactI18next)
  .init({
    resources: {
      en: { translation: en },
      ar: { translation: ar },
      fr: { translation: fr }
    },

    lng: savedLang,          // ✅ use saved language
    fallbackLng: "en",

    debug: false,            // 🔥 set true if you want to debug

    interpolation: {
      escapeValue: false
    }
  });

// ✅ RTL SUPPORT (VERY IMPORTANT FOR ARABIC)
const setDirection = (lang) => {
  document.documentElement.dir = lang === "ar" ? "rtl" : "ltr";
};

// run once on load
setDirection(i18n.language);

// run every time language changes
i18n.on("languageChanged", (lng) => {
  localStorage.setItem("lang", lng);   // ✅ persist language
  setDirection(lng);                   // ✅ update direction
});

export default i18n;