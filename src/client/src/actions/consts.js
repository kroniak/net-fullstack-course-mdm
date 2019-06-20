const BACKEND_URL = process.env.NODE_ENV === "production" ? "http://localhost:5001" : "http://localhost:5000";
export const ROOT_URL = BACKEND_URL + "/api/v1";
