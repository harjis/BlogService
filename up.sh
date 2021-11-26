#!/usr/bin/env bash

set -o errexit
set -o pipefail
set -o nounset

printf "Install DB\n\n"
./db/install.sh
printf "\n\nBuild migration images\n\n"
./build-local.sh
printf "\n\nMigrate DB\n\n"
./db/migrate.sh

# Notice! Db needs to be migrated before kafka is installed
# Otherwise the source connector in Kafka Connect will fail and
# not work correctly because Outbox-table is missing
printf "\n\nInstall kafka\n\n"
./kafka/install.sh