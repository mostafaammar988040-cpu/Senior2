import React from "react";
import "../styles/TaxiServices.css";
import { useTranslation } from "react-i18next";

const SERVICES = [
  {
    id: "bolt",
    name: "Bolt",
    city: "Lebanon",
    image: "/images/bolt.jpg",
    app: {
      android: "https://play.google.com/store/apps/details?id=ee.mtakso.client&hl=en",
      ios: "https://apps.apple.com/ee/app/bolt-request-a-ride/id675033630",
    },
    phone: null,
    whatsapp: null,
  },
  {
    id: "allo",
    name: "Allo Taxi",
    city: "Beirut",
    image: "/images/allotaxi.jpg",
    app: {
      android: "https://play.google.com/store/search?q=allo+taxi&c=apps&hl=en",
      ios: "https://apps.apple.com/ee/app/allo-taxi-lebanon/id897817091",
    },
    phone: "1213",
    whatsapp: "96178881213",
  },
  {
    id: "white",
    name: "White Taxi",
    city: "Beirut",
    image: "/images/whitetaxi.jpg",
    app: {
      android: "https://play.google.com/store/search?q=white%20taxi&c=apps&hl=en",
      ios: "https://apps.apple.com/ee/app/white-taxi-lb/id6743743011",
    },
    phone: "+96101513595",
    whatsapp: "96171722199",
  },
  {
    id: "red",
    name: "Red Taxi",
    city: "Beirut",
    image: "/images/redtaxi.jpg",
    app: null,
    phone: "1579",
    whatsapp: null,
  },
  {
    id: "charlie",
    name: "Charlie Taxi",
    city: "Beirut",
    image: "/images/charlietaxi.jpg",
    app: null,
    phone: "+96101285710",
    whatsapp: null,
  },
];

function waLink(number, text) {
  const msg = text ? `?text=${encodeURIComponent(text)}` : "";
  return `https://wa.me/${number}${msg}`;
}

function telLink(phone) {
  return `tel:${phone}`;
}

function getBestStoreLink(app) {
  if (!app) return null;
  return app.android || app.ios || null;
}

export default function TaxiServices() {
  const { t } = useTranslation();

  return (
    <div className="taxiPage">
      <div className="taxiContainer">

        <header className="taxiHeader">
          <div>
            <div className="taxiKicker">{t("taxi.kicker")}</div>
            <h1 className="taxiTitle">{t("taxi.title")}</h1>
            <p className="taxiSub">{t("taxi.subtitle")}</p>
          </div>

          <div className="taxiTip">
            <span className="taxiTipLabel">{t("taxi.tipLabel")}</span>
            {t("taxi.tip")}
          </div>
        </header>

        <section className="taxiGrid">
          {SERVICES.map((s) => {
            const storeLink = getBestStoreLink(s.app);

            return (
              <article key={s.id} className="taxiCard">

                <div className="taxiImage">
                  <img src={s.image} alt={s.name} />
                </div>

                <div className="taxiContent">

                  <div className="taxiNameRow">
                    <h2>{s.name}</h2>
                    <span className="taxiCity">{s.city}</span>
                  </div>

                  <div className="taxiButtons">

                    {storeLink && (
                      <a className="btn primary" href={storeLink} target="_blank" rel="noreferrer">
                        {t("taxi.openApp")}
                      </a>
                    )}

                    {s.whatsapp && (
                      <a
                        className="btn"
                        href={waLink(
                          s.whatsapp,
                          t("taxi.whatsappMessage")
                        )}
                        target="_blank"
                        rel="noreferrer"
                      >
                        {t("taxi.whatsapp")}
                      </a>
                    )}

                    {s.phone && (
                      <a className="btn" href={telLink(s.phone)}>
                        {t("taxi.call")}
                      </a>
                    )}

                  </div>

                  <div className="taxiFine">
                    {s.phone && (
                      <>
                        <span className="taxiFineLabel">{t("taxi.phone")}:</span> {s.phone}
                      </>
                    )}

                    {s.phone && s.whatsapp && <span className="taxiSep">•</span>}

                    {s.whatsapp && (
                      <>
                        <span className="taxiFineLabel">{t("taxi.whatsapp")}:</span> +{s.whatsapp}
                      </>
                    )}
                  </div>

                </div>

              </article>
            );
          })}
        </section>

      </div>
    </div>
  );
}