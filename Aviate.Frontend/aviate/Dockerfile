# =========================
# BASE
# =========================
FROM node:20-alpine AS base
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .

# =========================
# DEV
# =========================
FROM base AS dev
ENV NODE_ENV=development
ENV PORT=3004
EXPOSE 3004
CMD ["npm", "run", "dev"]

# =========================
# PROD BUILD
# =========================
FROM base AS build
RUN npm run build

# =========================
# PROD RUN
# =========================
FROM node:20-alpine AS prod
WORKDIR /app
ENV NODE_ENV=production
ENV PORT=3004
COPY --from=build /app/.next ./.next
COPY --from=build /app/public ./public
COPY --from=build /app/package*.json ./
RUN npm install --omit=dev
EXPOSE 3004
CMD ["npm", "start"]
