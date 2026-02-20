// TaxiServices.jsx
import React from "react";
import "../styles/TaxiServices.css";

const SERVICES = [
  // Bolt: app only
  {
    id: "bolt",
    name: "Bolt",
    city: "Lebanon",
    app: {
      android: "https://play.google.com/store/apps/details?id=ee.mtakso.client&hl=en", // TODO: replace with Bolt Play link
      ios: "https://apps.apple.com/ee/app/bolt-request-a-ride/id675033630", // TODO: replace with Bolt App Store link
    },
    phone: null,
    whatsapp: null,
  },

  // Allo Taxi: whatsapp + app + call
  {
    id: "allo",
    name: "Allo Taxi",
    city: "Beirut",
    app: {
      android: "https://play.google.com/store/search?q=allo+taxi&c=apps&hl=en", // TODO: replace
      ios: "https://apps.apple.com/ee/app/allo-taxi-lebanon/id897817091", // TODO: replace
    },
    phone: "1213", // TODO: replace
    whatsapp: "96178881213", // TODO: replace (digits only, no +)
  },

  // White Taxi: whatsapp + app + call
  {
    id: "white",
    name: "White Taxi",
    city: "Beirut",
    app: {
      android: "https://play.google.com/store/search?q=white%20taxi&c=apps&hl=en", // TODO: replace
      ios: "https://apps.apple.com/ee/app/white-taxi-lb/id6743743011", // TODO: replace
    },
    phone: "+96101513595", // TODO: replace
    whatsapp: "96171722199", // TODO: replace
  },

  // Red Taxi: app + call
  {
    id: "red",
    name: "Red Taxi",
    city: "Beirut",
    app: {
      android: null, // TODO: replace
      ios: null, // TODO: replace
    },
    phone: "1579", // TODO: replace
    whatsapp: null,
  },

  // Charlie Taxi: whatsapp + call
  {
    id: "charlie",
    name: "Charlie Taxi",
    city: "Beirut",
    app: null,
    phone: "+96101285710", // TODO: replace
    whatsapp: null, // TODO: replace
  },
];

function waLink(e164digits, prefillText) {
  const text = prefillText ? `?text=${encodeURIComponent(prefillText)}` : "";
  return `https://wa.me/${String(e164digits).replace(/\D/g, "")}${text}`;
}

function telLink(phone) {
  return `tel:${String(phone).replace(/\s/g, "")}`;
}

function getBestStoreLink(app) {
  // Simple approach: prefer Android link if present, else iOS link
  // (You can improve later by detecting iOS/Android)
  if (!app) return null;
  return app.android || app.ios || null;
}

export default function TaxiServices() {
  return (
    <div className="taxiPage">
      <header className="taxiHeader">
        <div>
          <div className="taxiKicker">Transportation</div>
          <h1 className="taxiTitle">Taxi Services in Lebanon</h1>
          <p className="taxiSub">
            Fast access to each service’s available contact method (App, WhatsApp, Call).
          </p>
        </div>

        <div className="taxiTip">
          <span className="taxiTipLabel">Tip</span>
          Agree on the price before starting the ride (especially outside Beirut).
        </div>
      </header>

      <section className="taxiGrid">
        {SERVICES.map((s) => {
          const storeLink = getBestStoreLink(s.app);

          return (
            <article key={s.id} className="taxiCard">
              <div className="taxiCardTop">
                <div className="taxiAvatar" aria-hidden="true">
                  {s.name
                    .split(" ")
                    .slice(0, 2)
                    .map((w) => w[0]?.toUpperCase())
                    .join("")}
                </div>

                <div className="taxiInfo">
                  <div className="taxiNameRow">
                    <div className="taxiName">{s.name}</div>
                    <div className="taxiCity">{s.city}</div>
                  </div>
                  <div className="taxiMeta">
                    Available:
                    {storeLink ? <span className="taxiMetaItem">App</span> : null}
                    {s.whatsapp ? <span className="taxiMetaItem">WhatsApp</span> : null}
                    {s.phone ? <span className="taxiMetaItem">Call</span> : null}
                  </div>
                </div>
              </div>

              <div className="taxiButtons">
                {storeLink ? (
                  <a className="btn primary" href={storeLink} target="_blank" rel="noreferrer">
                    Open App
                  </a>
                ) : null}

                {s.whatsapp ? (
                  <a
                    className="btn"
                    href={waLink(
                      s.whatsapp,
                      "Hello! I need a taxi. Pickup: ____ Destination: ____ Time: ____"
                    )}
                    target="_blank"
                    rel="noreferrer"
                  >
                    WhatsApp
                  </a>
                ) : null}

                {s.phone ? (
                  <a className="btn" href={telLink(s.phone)}>
                    Call
                  </a>
                ) : null}
              </div>

              {(s.phone || s.whatsapp) ? (
                <div className="taxiFine">
                  {s.phone ? (
                    <>
                      <span className="taxiFineLabel">Phone:</span> {s.phone}
                    </>
                  ) : null}
                  {s.phone && s.whatsapp ? <span className="taxiSep">•</span> : null}
                  {s.whatsapp ? (
                    <>
                      <span className="taxiFineLabel">WhatsApp:</span> +{s.whatsapp}
                    </>
                  ) : null}
                </div>
              ) : null}
            </article>
          );
        })}
      </section>
    </div>
  );
}